using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mail;
using System.Web.UI.WebControls;
using Esperantus;
using Rainbow.Configuration;
using Rainbow.DesktopModules.Newsletter;
using Rainbow.Helpers;
using Rainbow.Settings;
using Rainbow.UI.DataTypes;
using Rainbow.UI.WebControls;
using CheckBox = Esperantus.WebControls.CheckBox;
using Label = Esperantus.WebControls.Label;
using LinkButton = Esperantus.WebControls.LinkButton;
using Literal = Esperantus.WebControls.Literal;
using RegularExpressionValidator = Esperantus.WebControls.RegularExpressionValidator;
using RequiredFieldValidator = Esperantus.WebControls.RequiredFieldValidator;

namespace Rainbow.DesktopModules
{

    /// <summary>
    ///	Summary description for SendNewsletter.
    /// </summary>
    public class SendNewsletter : PortalModuleControl
    {
        string InvalidRecipients = string.Empty;

        protected string ServerVariables;
		protected string Titulo;
        protected DataList DataList1;
        protected TextBox txtEMail;
        protected TextBox txtName;
        protected TextBox txtSubject;
        protected TextBox txtBody;
		protected RequiredFieldValidator validEmailRequired;
		protected RegularExpressionValidator validEmailRegExp;
        protected Panel EditPanel;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected CheckBox HtmlMode;
        protected Panel UsersPanel;
		protected Literal Literal3;
		protected Literal Literal2;
		protected Literal Literal1;
		protected CheckBox InsertBreakLines;
		protected Literal Literal5;
		protected Literal Literal6;
		protected Literal Literal7;
		protected Literal Literal8;
		protected LinkButton previewButton;
		protected LinkButton cancelButton;
		protected Label Label1;
		protected System.Web.UI.WebControls.Label lblFrom;
		protected Label Label2;
		protected System.Web.UI.WebControls.Label lblTo;
		protected Label Label3;
		protected System.Web.UI.WebControls.Label lblSubject;
		protected System.Web.UI.WebControls.Label lblBody;
		protected LinkButton submitButton;
		protected LinkButton cancelButton2;
		protected Panel PrewiewPanel;
		protected System.Web.UI.WebControls.Label CreatedDate;
		protected Literal Literal4;

        private void Page_Load(object sender, EventArgs e)
        {
			// Added EsperantusKeys for Localization 
			// Mario Endara mario@softworks.com.uy 11/05/2004 
			Titulo = Localize.GetString ("NEWSLETTER_TITLE");

            if (!Page.IsPostBack)
			{				
				CreatedDate.Text = DateTime.Now.ToLongDateString();

				//Set default
				txtName.Text = Settings["NEWSLETTER_DEFAULTNAME"].ToString();
				txtEMail.Text = Settings["NEWSLETTER_DEFAULTEMAIL"].ToString();
				if (txtEMail.Text == string.Empty)
					txtEMail.Text = PortalSettings.CurrentUser.Identity.Email;

				//create a DataTable
				DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("StringValue"));
            
                NewsletterDB newsletter = new NewsletterDB();

				if (!bool.Parse(Settings["TestMode"].ToString()))
				{
					SqlDataReader dReader = newsletter.GetUsersNewsletter(portalSettings.PortalID, Int32.Parse(Settings["NEWSLETTER_USERBLOCK"].ToString()), Int32.Parse(Settings["NEWSLETTER_DONOTRESENDWITHIN"].ToString()));
					try
					{
						while (dReader.Read())
						{
							DataRow dr = dt.NewRow();
							dr[0] = "<b>" + dReader["Name"].ToString() + ":</b> " + dReader["EMail"].ToString(); 
							dt.Rows.Add(dr);    
						}
					}
					finally
					{
						dReader.Close(); //by Manu, fixed bug 807858
					}
	
					DataList1.DataSource = new DataView(dt);
					DataList1.RepeatDirection = RepeatDirection.Vertical;
					DataList1.RepeatLayout = RepeatLayout.Table;
					DataList1.BorderWidth = Unit.Pixel(1);
					DataList1.GridLines = GridLines.Both;
					DataList1.RepeatColumns = 3;
					DataList1.DataBind();
					DataList1.Visible = true;
                
					int cnt = newsletter.GetUsersNewsletterCount(portalSettings.PortalID, Int32.Parse(Settings["NEWSLETTER_USERBLOCK"].ToString()), Int32.Parse(Settings["NEWSLETTER_DONOTRESENDWITHIN"].ToString()));
					// Added EsperantusKeys for Localization 
					// Mario Endara mario@softworks.com.uy 11/05/2004 
					lblMessage.Text = Localize.GetString ("NEWSLETTER_MSG").Replace("{1}", cnt.ToString ());
				}
				else
				{
					// Added EsperantusKeys for Localization 
					// Mario Endara mario@softworks.com.uy 11/05/2004 
					lblMessage.Text = Localize.GetString("NEWSLETTER_MSG_TEST").Replace("{1}", txtName.Text).Replace("{2}", txtEMail.Text);
				}

				//Try to get template
				int HTMLModID = int.Parse(Settings["NEWSLETTER_HTMLTEMPLATE"].ToString());
				if (HTMLModID > 0)
				{
					// Obtain the selected item from the HtmlText table
					NewsletterHtmlTextDB text = new NewsletterHtmlTextDB();
					SqlDataReader dr = text.GetHtmlText(HTMLModID, WorkFlowVersion.Staging);
					try
					{
						if (dr.Read())
						{
							string buffer = (string) dr["DesktopHtml"];
							// Replace relative path to absolute path. jviladiu@portalServices.net 19/07/2004
							buffer = buffer.Replace (Path.ApplicationFullPath, Path.ApplicationRoot);
							if (Path.ApplicationRoot.Length > 0) //by Manu... on root PortalSettings.ApplicationPath is empty
								buffer = buffer.Replace (Path.ApplicationRoot, Path.ApplicationFullPath);

							txtBody.Text = Server.HtmlDecode(buffer);
							HtmlMode.Checked = true;
						}
						else 
							HtmlMode.Checked = false;
					}
					finally
					{
						dr.Close();
					}
				}
			}
            EditPanel.Visible = true;
            PrewiewPanel.Visible = false;
            UsersPanel.Visible = true;
        }

        /// <summary>
        /// The CancelBtn_Click server event handler on this page is used
        /// to handle the scenario where a user clicks the "cancel"
        /// button to discard a message post and toggle out of edit mode.
        /// </summary>
		private void cancelButton_Click(object sender, EventArgs e)
		{
			txtSubject.Text = string.Empty;
			txtBody.Text = string.Empty;
		}

        /// <summary>
        /// The SubmitBtn_Click server event handler on this page is used
        /// to handle the scenario where a user clicks the "send"
        /// button after entering a response to a message post.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void submitButton_Click(object sender, EventArgs e)
		{
            string message;
            int cnt = 0;
            EditPanel.Visible = false;
            PrewiewPanel.Visible = true;
            UsersPanel.Visible = false;

        	SmtpMail.SmtpServer = Config.SmtpServer;

            message = Localize.GetString("NEWSLETTER_SENDTO", "<b>Message:</b> sent to:<br>");

			try
			{
				NewsletterDB newsletter = new NewsletterDB();
				if (!bool.Parse(Settings["TestMode"].ToString()))
				{
					// Get Newsletter Users from DB
					SqlDataReader dReader = newsletter.GetUsersNewsletter(portalSettings.PortalID, Int32.Parse(Settings["NEWSLETTER_USERBLOCK"].ToString()), Int32.Parse(Settings["NEWSLETTER_DONOTRESENDWITHIN"].ToString()));
					try
					{
						while (dReader.Read())
						{
							cnt++;
							message += dReader["Email"].ToString() + ", ";
							try
							{
								//Send the email
								newsletter.SendMessage(txtEMail.Text, dReader["Email"].ToString(), dReader["Name"].ToString(), dReader["Password"].ToString(), Settings["NEWSLETTER_LOGINHOMEPAGE"].ToString(), txtSubject.Text, txtBody.Text, true, HtmlMode.Checked, InsertBreakLines.Checked);
								//Update db
								newsletter.SendNewsletterTo(portalSettings.PortalID, dReader["Email"].ToString());
							}
							catch(Exception ex)
							{
								InvalidRecipients += dReader["Email"].ToString() + "<br>";
								BlacklistDB.AddToBlackList(portalSettings.PortalID, dReader["Email"].ToString(), ex.Message);
							}
						}
					}
					finally
					{
						dReader.Close(); //by Manu, fixed bug 807858
					}
					lblMessage.Text = Localize.GetString("NEWSLETTER_SENDINGTO", "Message has been sent to {1} registered users.").Replace("{1}", cnt.ToString());
				}
				else
				{
					newsletter.SendMessage(txtEMail.Text, txtEMail.Text, txtName.Text, "******", Settings["NEWSLETTER_LOGINHOMEPAGE"].ToString(), txtSubject.Text, txtBody.Text, true, HtmlMode.Checked, InsertBreakLines.Checked);
					lblMessage.Text = Localize.GetString("NEWSLETTER_TESTSENDTO", "Test message sent to: ") + txtName.Text + " [" + txtEMail.Text + "]";
				}
			}
			catch (Exception ex)
			{
				lblMessage.Text = Localize.GetString("NEWSLETTER_ERROR", "An error occurred: ") + ex.Message;
			}
            
			CreatedDate.Text =  Localize.GetString("NEWSLETTER_SENDDATE", "Message sent: ") + DateTime.Now.ToLongDateString() + "<br>";

            if (InvalidRecipients.Length>0)
                CreatedDate.Text +=  Localize.GetString("NEWSLETTER_INVALID_RECIPIENTS", "Invalid recipients:<br>") + InvalidRecipients;

            //Hides commands
            submitButton.Visible = false;
            cancelButton2.Visible = false;
        }

		/// <summary>
		/// Admin Module
		/// </summary>
		public override bool AdminModule
		{
			get
			{
				return true;
			}
		}

        public override Guid GuidID 
        {
            get
            {
                return new Guid("{B484D450-5D30-4C4B-817C-14A25D06577E}");
            }
        }
          
		#region Web Form Designer generated code
		/// <summary>
		/// Raises Init event
		/// </summary>
		/// <param name="e"></param>
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		///<summary>
		///	Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		///</summary>
        private void InitializeComponent()
        {
			this.previewButton.Click += new EventHandler(this.previewButton_Click);
			this.cancelButton.Click += new EventHandler(this.cancelButton_Click);
			this.submitButton.Click += new EventHandler(this.submitButton_Click);
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion

        public SendNewsletter()
        {
			// modified by Hongwei Shen
			SettingItemGroup group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
			int groupBase = (int)group;

			SettingItem DefaultEmail = new SettingItem(new StringDataType());
			DefaultEmail.EnglishName = "Sender Email";
			DefaultEmail.Group = group;
			DefaultEmail.Order = groupBase + 20;
			_baseSettings.Add("NEWSLETTER_DEFAULTEMAIL", DefaultEmail);

			SettingItem DefaultName = new SettingItem(new StringDataType());
			DefaultName.Group = group;
			DefaultName.Order = groupBase + 25;
			DefaultName.EnglishName = "Sender Name";
			_baseSettings.Add("NEWSLETTER_DEFAULTNAME", DefaultName);

			SettingItem TestMode = new SettingItem(new BooleanDataType());
			TestMode.Value = "False";
			TestMode.Group = group;
			TestMode.Order = groupBase + 30;
			TestMode.EnglishName = "Test Mode";
			TestMode.Description = "Use Test Mode for testing your settings. It will send only one email to the address specified as sender. Useful for testing";
			_baseSettings.Add("TestMode", TestMode);

			//SettingItem HTMLTemplate = new SettingItem(new CustomListDataType(new Rainbow.Configuration.ModulesDB().GetModuleDefinitionByGuid(p, new Guid("{0B113F51-FEA3-499A-98E7-7B83C192FDBB}")), "ModuleTitle", "ModuleID"));
			SettingItem HTMLTemplate = new SettingItem(new ModuleListDataType("Html Document"));
			HTMLTemplate.Value = "0";
			HTMLTemplate.Group = group;
			HTMLTemplate.Order = groupBase + 35;
			HTMLTemplate.EnglishName = "HTML Template";
			HTMLTemplate.Description = "Select an HTML module that the will be used as base for this newsletter sent with this module.";
			_baseSettings.Add("NEWSLETTER_HTMLTEMPLATE", HTMLTemplate);

			SettingItem LoginHomePage = new SettingItem(new StringDataType());
			LoginHomePage.EnglishName = "Site URL";
			LoginHomePage.Group = group;
			LoginHomePage.Order = groupBase + 40;
			LoginHomePage.Description = "The Url or the Home page of the site used for build the instant login url. Leave blank if using the current site.";
			_baseSettings.Add("NEWSLETTER_LOGINHOMEPAGE", LoginHomePage);

			SettingItem DoNotResendWithin = new SettingItem(new ListDataType("1;2;3;4;5;6;7;10;15;30;60;90"));
			DoNotResendWithin.Value = "7";
			DoNotResendWithin.Group = group;
			DoNotResendWithin.Order = groupBase + 45;
			DoNotResendWithin.EnglishName = "Do not resend within (days)";
			DoNotResendWithin.Description = "To avoid spam and duplicate sent you cannot email an user more than one time in specifed number of days.";
			_baseSettings.Add("NEWSLETTER_DONOTRESENDWITHIN", DoNotResendWithin);

			SettingItem UserBlock = new SettingItem(new ListDataType("50;100;200;250;300;1000;1500;5000"));
			UserBlock.EnglishName = "Group users";
			UserBlock.Group = group;
			UserBlock.Order = groupBase + 50;
			UserBlock.Description = "Select the maximum number of users. For sending emails to all you have to repeat the process. Use small values to avoid server timeouts.";
			UserBlock.Value = "250";
			_baseSettings.Add("NEWSLETTER_USERBLOCK", UserBlock);
        }

        /// <summary>
        /// The previewButton_Click server event handler on this page is used
        /// to handle the scenario where a user clicks the "preview"
        /// button to see a preview of the message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewButton_Click(object sender, EventArgs e)
        {
	        //SmtpMail.SmtpServer = Portal.SmtpServer;
			SmtpMail.SmtpServer = Config.SmtpServer;

            CreatedDate.Text =  Localize.GetString("NEWSLETTER_LOCALTIME", "Local time: ") + DateTime.Now.ToLongDateString();

            NewsletterDB newsletter = new NewsletterDB();

			string email;
			string name;
			string password;

			if (!bool.Parse(Settings["TestMode"].ToString()))
			{
				SqlDataReader dReader = newsletter.GetUsersNewsletter(portalSettings.PortalID, Int32.Parse(Settings["NEWSLETTER_USERBLOCK"].ToString()), Int32.Parse(Settings["NEWSLETTER_DONOTRESENDWITHIN"].ToString()));
				try
				{
					if (dReader.Read())
					{
						email = dReader["Email"].ToString();
						name = dReader["Name"].ToString();
						password = dReader["Password"].ToString();
					}
					else
					{
						lblMessage.Text = Localize.GetString("NEWSLETTER_NORECIPIENTS", "No recipients");
						return; //nothing more to do here
					}
				}
				finally
				{
					dReader.Close(); //by Manu, fixed bug 807858
				}
			}
			else
			{
				email = txtEMail.Text;
				name = txtName.Text;
				password = "*******"; //Fake password
			}

			EditPanel.Visible = false;
			PrewiewPanel.Visible = true;
			UsersPanel.Visible = false;
			lblFrom.Text = txtName.Text + " (" + txtEMail.Text + ")";
			lblTo.Text = name + " (" + email + ")";
			lblSubject.Text = txtSubject.Text;
			string body = newsletter.SendMessage(txtEMail.Text, email, name, password, Settings["NEWSLETTER_LOGINHOMEPAGE"].ToString(), txtSubject.Text, txtBody.Text, false, HtmlMode.Checked, InsertBreakLines.Checked);
			if(HtmlMode.Checked)
			{
				lblBody.Text = body;
			}
			else
			{
				lblBody.Text = "<PRE>" + body + "</PRE>";
			}
        }

		# region Install / Uninstall Implementation
		public override void Install(IDictionary stateSaver)
		{
			string currentScriptName = Server.MapPath(this.TemplateSourceDirectory + "/Newsletter_Install.sql");
			ArrayList errors = DBHelper.ExecuteScript(currentScriptName, true);
			if (errors.Count > 0)
			{
				// Call rollback
				throw new Exception("Error occurred:" + errors[0].ToString());
			}
		}

		public override void Uninstall(IDictionary stateSaver)
		{
			string currentScriptName = Server.MapPath(this.TemplateSourceDirectory + "/Newsletter_Uninstall.sql");
			ArrayList errors = DBHelper.ExecuteScript(currentScriptName, true);
			if (errors.Count > 0)
			{
				// Call rollback
				throw new Exception("Error occurred:" + errors[0].ToString());
			}
		}
		#endregion

    }
}