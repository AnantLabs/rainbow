
		
//========================================================================
// This file was generated using the MyGeneration tool in combination
// with the Gentle.NET Business Entity template, $Rev: 44 $
//========================================================================
using System;
using System.Collections;
using Gentle.Framework;

namespace Rainbow.Data.GentleNET
{
	#region rb_Surveys
	/// <summary>
	/// This object represents the properties and methods of a Employee.
	/// </summary>
	[Serializable]
	[TableName("rb_Surveys")]
	public class rb_Surveys : Persistent
	{
		#region Members
		private bool _changed = false;
		private static bool invalidatedListAll = true;
		private static ArrayList listAllCache = null;
		[TableColumn("SurveyID", NotNull=true), PrimaryKey(AutoGenerated=true)]
		protected int surveyID;
		[TableColumn("ModuleID", NotNull=true), ForeignKey("rb_Modules", "ModuleID")]
		protected int moduleID;
		[TableColumn("SurveyDesc", NotNull=true)]
		protected string surveyDesc;
		[TableColumn("CreatedByUser", NotNull=true)]
		protected string createdByUser;
		[TableColumn("CreatedDate", NotNull=true)]
		protected DateTime createdDate;
		#endregion
			

		#region Constructors
	

		/// <summary> 
		/// Create a new object by specifying all fields (except the auto-generated primary key field). 
		/// </summary> 
		public rb_Surveys( 
				int ModuleID, 
				string SurveyDesc, 
				string CreatedByUser, 
				DateTime CreatedDate)
		{
			_changed = true;
			invalidatedListAll = true;
			moduleID = ModuleID;
			surveyDesc = SurveyDesc;
			createdByUser = CreatedByUser;
			createdDate = CreatedDate;
		}

			
		/// <summary> 
		/// Create an object from an existing row of data. This will be used by Gentle to 
		/// construct objects from retrieved rows. 
		/// </summary> 
		public rb_Surveys( 
				int SurveyID, 
				int ModuleID, 
				string SurveyDesc, 
				string CreatedByUser, 
				DateTime CreatedDate)
		{
			surveyID = SurveyID;
			moduleID = ModuleID;
			surveyDesc = SurveyDesc;
			createdByUser = CreatedByUser;
			createdDate = CreatedDate;
		}

		#endregion

		#region Public Properties
		
		public bool Changed
		{ get { return _changed; } }
		
		public int SurveyID
		{
			get{ return surveyID; }
		}
		
		public int ModuleID
		{
			get{ return moduleID; }
			set{ _changed |= moduleID != value; moduleID = value; invalidatedListAll =  _changed;}
		}
		
		public string SurveyDesc
		{
			get{ return surveyDesc != null ?surveyDesc.TrimEnd() : null; }
			set{ _changed |= surveyDesc != value; surveyDesc = value; invalidatedListAll =  _changed;}
		}
		
		public string CreatedByUser
		{
			get{ return createdByUser != null ?createdByUser.TrimEnd() : null; }
			set{ _changed |= createdByUser != value; createdByUser = value; invalidatedListAll =  _changed;}
		}
		
		public DateTime CreatedDate
		{
			get{ return createdDate; }
			set{ _changed |= createdDate != value; createdDate = value; invalidatedListAll =  _changed;}
		}
		
	
		// generate a static property to retrieve all instances of a class that are stored in the database
		static public IList ListAll
		{
			get 
			{ 
				if( listAllCache == null || invalidatedListAll )
				{
					listAllCache = Broker.RetrieveList( typeof(rb_Surveys) ) as ArrayList;
					invalidatedListAll = false;
				}
				return listAllCache;
			}
		}
		
		#endregion

		#region Debug Properties
		/// <summary>
		/// Generated only when genDebug flag enabled in MyGeneration template UI
		/// Returns number of items in internal cache
		/// </summary>
		public static int CacheCount
		{
			get{ return listAllCache == null ? 0 : listAllCache.Count;}
		}
		
		#endregion

		#region Storage and retrieval
	   
		public static rb_Surveys Retrieve(int id )
		{
	   
			// Return null if id is smaller than seed and/or increment for autokey
			if(id<0) 
			{
				return null;
			}
		  
			Key key = new Key( typeof(rb_Surveys), true, "SurveyID", id );
			return Broker.RetrieveInstance( typeof(rb_Surveys), key ) as rb_Surveys;
		}

		//Gentle.NET Business Entity script: Generation of complex retrieve function (multiple primary keys) is not implemented yet.

		public override void Persist()
		{
			if( Changed || !IsPersisted )
			{
				base.Persist();
				_changed=false;
			}
		}

		public override void Remove()
		{
			base.Remove();
			invalidatedListAll = true;
		}
		#endregion

		#region Relations
		// List of primary keys for this class table
		// Key: SurveyID
		// List of foreign keys for this class table
		// Key: FK_rb_SurveyAnswers_rb_Surveys primary table: rb_Surveys primary column: SurveyID foreign column: SurveyID foreign table: rb_SurveyAnswers
		// Key: FK_rb_SurveyQuestions_rb_Surveys primary table: rb_Surveys primary column: SurveyID foreign column: SurveyID foreign table: rb_SurveyQuestions
		// Key: FK_rb_Surveys_Modules primary table: rb_Modules primary column: ModuleID foreign column: ModuleID foreign table: rb_Surveys
		// List of selected relation tables for this database
		// Table: rb_Announcements
		// Table: rb_Announcements_st
		// Table: rb_Articles
		// Table: rb_Blacklist
		// Table: rb_BlogComments
		// Table: rb_Blogs
		// Table: rb_BlogStats
		// Table: rb_BookList
		// Table: rb_ComponentModule
		// Table: rb_Contacts
		// Table: rb_Contacts_st
		// Table: rb_ContentManager
		// Table: rb_Countries
		// Table: rb_Cultures
		// Table: rb_Discussion
		// Table: rb_Documents
		// Table: rb_Documents_st
		// Table: rb_EnhancedHtml
		// Table: rb_EnhancedHtml_st
		// Table: rb_EnhancedLinks
		// Table: rb_EnhancedLinks_st
		// Table: rb_Events
		// Table: rb_Events_st
		// Table: rb_FAQs
		// Table: rb_GeneralModuleDefinitions
		// Table: rb_HtmlText
		// Table: rb_HtmlText_st
		// Table: rb_Links
		// Table: rb_Links_st
		// Table: rb_Localize
		// Table: rb_Milestones
		// Table: rb_Milestones_st
		// Table: rb_ModuleDefinitions
		// Table: rb_Modules
		// Table: rb_ModuleSettings
		// Table: rb_Monitoring
		// Table: rb_Pictures
		// Table: rb_Pictures_st
		// Table: rb_Portals
		// Table: rb_PortalSettings
		// Table: rb_Roles
		// Table: rb_SolutionModuleDefinitions
		// Table: rb_Solutions
		// Table: rb_States
		// Table: rb_SurveyAnswers
		// Table: rb_SurveyOptions
		// Table: rb_SurveyQuestions
		// Table: rb_Surveys
		// Table: rb_Tabs
		// Table: rb_TabSettings
		// Table: rb_Tasks
		// Table: rb_UserDefinedData
		// Table: rb_UserDefinedFields
		// Table: rb_UserDefinedRows
		// Table: rb_UserDesktop
		// Table: rb_UserRoles
		// Table: rb_Users
		// Table: rb_Versions

		/// <summary>
		/// Return list of referenced objects from n:m relation with
		/// table "rb_SurveyOptions", using relation table "rb_SurveyQuestions"
		/// </summary>
		public IList referencedrb_SurveyOptions()
		{
			return new GentleList( typeof(rb_SurveyOptions), this, typeof(rb_SurveyQuestions));
		}

		/// <summary>
		/// Return list of referenced objects from n:m relation with
		/// table "rb_SurveyAnswers", using relation table "rb_Surveys"
		/// </summary>
		public IList referencedrb_SurveyAnswers()
		{
			return new GentleList( typeof(rb_SurveyAnswers), this, typeof(rb_Surveys));
		}

		/// <summary>
		/// Return list of referenced objects from n:m relation with
		/// table "rb_SurveyQuestions", using relation table "rb_Surveys"
		/// </summary>
		public IList referencedrb_SurveyQuestions()
		{
			return new GentleList( typeof(rb_SurveyQuestions), this, typeof(rb_Surveys));
		}

		/// <summary>
		/// Return list of referenced objects from n:m relation with
		/// table "rb_Modules", using relation table "rb_Surveys"
		/// </summary>
		public IList referencedrb_Modules()
		{
			return new GentleList( typeof(rb_Modules), this, typeof(rb_Surveys));
		}
		#endregion

		#region ManualCode
/***PRESERVE_BEGIN MANUAL_CODE***//***PRESERVE_END MANUAL_CODE***/
		#endregion
	}

}
#endregion


