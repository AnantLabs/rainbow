namespace Rainbow.Framework.Scheduler
{
    using System;
    using System.Collections;
    using System.Data;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;

    // Author: Federico Dal Maso
    // e-mail: ifof@libero.it
    // date: 2003-06-17

    /// <summary>
    /// Summary description for SchedulerDB.
    /// </summary>
    internal class SchedulerDB
    {
        #region Constants and Fields

        /// <summary>
        /// The app map path.
        /// </summary>
        private readonly string appMapPath;

        /// <summary>
        /// The database connection.
        /// </summary>
        private readonly IDbConnection cn;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerDB"/> class. 
        /// </summary>
        /// <param name="cn">
        /// The connection.
        /// </param>
        /// <param name="applicationMapPath">
        /// The application map path.
        /// </param>
        /// <returns>
        /// A void value...
        /// </returns>
        public SchedulerDB(IDbConnection cn, string applicationMapPath)
        {
            this.cn = cn;
            this.appMapPath = applicationMapPath;

            // should be: HttpContext.Current.Server.MapPath(PortalSettings.ApplicationPath)
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the expired task.
        /// </summary>
        /// <returns>
        /// A Rainbow.Framework.Scheduler.SchedulerTask[] value...
        /// </returns>
        public SchedulerTask[] GetExpiredTask()
        {
            var ary = new ArrayList();

            this.cn.Open();
            using (var cmd = this.cn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "rb_SchedulerGetExpiredTasks";

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ary.Add(new SchedulerTask(dr));
                    }

                    dr.Close();
                }
            }

            this.cn.Close();
            return (SchedulerTask[])ary.ToArray(typeof(SchedulerTask));
        }

        /// <summary>
        /// Gets the module instance.
        /// </summary>
        /// <param name="idModule">
        /// The id module.
        /// </param>
        /// <returns>
        /// A Rainbow.Framework.Scheduler.ISchedulable value...
        /// </returns>
        public ISchedulable GetModuleInstance(int idModule)
        {
            this.cn.Open();
            var cmd = this.cn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rb_SchedulerGetModuleClassName";
            IDataParameter par = cmd.CreateParameter();
            par.DbType = DbType.Int32;
            par.ParameterName = "@IDModule";
            par.Value = idModule;
            cmd.Parameters.Add(par);
            IDataReader dr;

            try
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                this.cn.Close();
                throw new SchedulerException("Unable to load assembly name from database", ex);
            }

            string assemblyName;
            string typeName;

            using (dr)
            {
                if (dr.Read())
                {
                    assemblyName = string.Concat(this.appMapPath, @"\bin\", (string)dr["AssemblyName"]);
                    typeName = (string)dr["ClassName"];
                }
                else
                {
                    throw new SchedulerException("Not assembly in database");
                }

                dr.Close(); // cn is closed by behavior.
            }

            Assembly a;

            try
            {
                a = Assembly.LoadFrom(assemblyName);
            }
            catch (Exception ex)
            {
                throw new SchedulerException("Cannot load assembly", ex);
            }

            object o;

            try
            {
                o = a.CreateInstance(typeName);
            }
            catch (Exception ex)
            {
                throw new SchedulerException(
                    string.Format("Unable to create instance of assembly: {0} , typeName: {1}", assemblyName, typeName), 
                    ex);
            }

            var module = o as ISchedulable;

            if (module == null)
            {
                throw new SchedulerException("Module don't implement ISchedulable interface");
            }

            return module;
        }

        /// <summary>
        /// Gets the ordered task.
        /// </summary>
        /// <returns>
        /// A System.Data.IDataReader value...
        /// </returns>
        public IDataReader GetOrderedTask()
        {
            this.cn.Open();
            var cmd = this.cn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rb_SchedulerGetOrderedTasks";
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Gets the tasks by owner.
        /// </summary>
        /// <param name="idOwner">
        /// The id owner.
        /// </param>
        /// <returns>
        /// A System.Data.IDataReader value...
        /// </returns>
        public IDataReader GetTasksByOwner(int idOwner)
        {
            this.cn.Open();
            var cmd = this.cn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rb_SchedulerGetTasksByOwner";
            IDbDataParameter par = cmd.CreateParameter();
            par.ParameterName = "@IDOwner";
            par.DbType = DbType.Int32;
            par.Direction = ParameterDirection.Input;
            par.Value = idOwner;
            cmd.Parameters.Add(par);
            var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }

        /// <summary>
        /// Gets the tasks by target.
        /// </summary>
        /// <param name="idTarget">
        /// The id target.
        /// </param>
        /// <returns>
        /// A System.Data.IDataReader value...
        /// </returns>
        public IDataReader GetTasksByTarget(int idTarget)
        {
            this.cn.Open();
            var cmd = this.cn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rb_SchedulerGetTasksByTarget";
            IDbDataParameter par = cmd.CreateParameter();
            par.ParameterName = "@IDTarget";
            par.DbType = DbType.Int32;
            par.Direction = ParameterDirection.Input;
            par.Value = idTarget;
            cmd.Parameters.Add(par);
            var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }

        /// <summary>
        /// Inserts the task.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <returns>
        /// A int value...
        /// </returns>
        public int InsertTask(SchedulerTask task)
        {
            if (task.DueTime < DateTime.Now)
            {
                throw new SchedulerException("Cannot schedule an expired task");
            }

            byte[] arg;

            using (var ss = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ss, task.Argument);
                arg = ss.ToArray();
                ss.Close();
            }

            int idTask;
            this.cn.Open();

            using (var cmd = this.cn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "rb_SchedulerAddTask";
                IDbDataParameter par;
                par = cmd.CreateParameter();
                par.ParameterName = "@IDOwner";
                par.DbType = DbType.Int32;
                par.Direction = ParameterDirection.Input;
                par.Value = task.IDModuleOwner;
                cmd.Parameters.Add(par);
                par = cmd.CreateParameter();
                par.ParameterName = "@IDTarget";
                par.DbType = DbType.Int32;
                par.Direction = ParameterDirection.Input;
                par.Value = task.IDModuleTarget;
                cmd.Parameters.Add(par);
                par = cmd.CreateParameter();
                par.ParameterName = "@DueTime";
                par.DbType = DbType.DateTime;
                par.Direction = ParameterDirection.Input;
                par.Value = task.DueTime;
                cmd.Parameters.Add(par);
                par = cmd.CreateParameter();
                par.ParameterName = "@Description";
                par.DbType = DbType.String;
                par.Size = 150;
                par.Direction = ParameterDirection.Input;
                par.Value = task.Description;
                cmd.Parameters.Add(par);
                par = cmd.CreateParameter();
                par.ParameterName = "@Argument";

                // par.DbType = DbType;
                par.Direction = ParameterDirection.Input;
                par.Value = arg;
                cmd.Parameters.Add(par);
                par = cmd.CreateParameter();
                par.ParameterName = "@IDTask";
                par.DbType = DbType.Int32;
                par.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(par);
                cmd.ExecuteNonQuery();
                idTask = (int)((IDataParameter)cmd.Parameters["@IDTask"]).Value;
            }

            this.cn.Close();

            if (idTask == -1)
            {
                throw new SchedulerException("Task add fail in DB");
            }

            return idTask;
        }

        /// <summary>
        /// Removes the task.
        /// </summary>
        /// <param name="iDTask">
        /// The i D task.
        /// </param>
        public void RemoveTask(int iDTask)
        {
            this.cn.Open();

            using (var cmd = this.cn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "rb_SchedulerRemoveTask";
                IDbDataParameter par;
                par = cmd.CreateParameter();
                par.ParameterName = "@IDTask";
                par.DbType = DbType.Int32;
                par.Direction = ParameterDirection.Input;
                par.Value = iDTask;
                cmd.Parameters.Add(par);
                cmd.ExecuteNonQuery();
            }

            this.cn.Close();
        }

        #endregion
    }
}