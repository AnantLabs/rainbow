using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Rainbow.Configuration;
using Rainbow.UI;
using Rainbow.UI.WebControls;
using CompareValidator = Esperantus.WebControls.CompareValidator;
using Literal = Esperantus.WebControls.Literal;
using RequiredFieldValidator = Esperantus.WebControls.RequiredFieldValidator;

namespace Rainbow.DesktopModules 
{
	/// <summary>
	/// Written by: Jos� Viladiu, jviladiu@portalServices.net
	/// based in original linkedit source
	/// </summary>
	public class EnhancedLinksEdit : AddEditItemPage
    {
		protected Literal Literal1;
		protected Literal Literal2;
		protected Literal Literal3;
		protected Literal Literal4;
		protected Literal Literal5;
		protected Literal Literal6;
		protected Literal Literal7;
		protected Literal Literal8;
		protected Literal Literal9;
		protected Literal Literal10;

		protected RequiredFieldValidator Req1;
		protected TextBox UrlField;
		protected RequiredFieldValidator Req2;
		protected TextBox MobileUrlField;
		protected TextBox DescriptionField;
		protected DropDownList TargetField;
		protected TextBox ViewOrderField;
		protected RequiredFieldValidator RequiredViewOrder;
		protected CompareValidator VerifyViewOrder;
		protected Label CreatedBy;
		protected RequiredFieldValidator Requiredfieldvalidator1;
		protected TextBox TitleField;
		protected Literal Literal11;
		protected UploadDialogTextBox Src;
		protected CheckBox IsGroup;
		protected Literal UrlFieldLabel;
		protected Literal MobileUrlFieldLabel;
		protected Literal TargetFieldLabel;
		protected Label CreatedDate;
		protected System.Web.UI.WebControls.Literal oldUrl;

		/// <summary>
		/// The Page_Load event on this Page is used to obtain the 
		/// ItemID of the link to edit.
		/// It then uses the Rainbow.EnhancedLinkDB() data component
		/// to populate the page's edit controls with the links details.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void Page_Load(object sender, EventArgs e) 
        {
			Src.FileNameOnly = true;
			Src.PreselectedFile = Src.Text;
			Src.UploadDirectory = portalSettings.PortalFullPath + "/" + moduleSettings["ENHANCEDLINKS_ICONPATH"].ToString();

			if (!Page.IsPostBack) 
            {
				TargetField.Items.Add("_new");
				TargetField.Items.Add("_blank");
				TargetField.Items.Add("_parent");
				TargetField.Items.Add("_self");
				TargetField.Items.Add("_top");

                if (ItemID != 0) 
                {
                    // Obtain a single row of link information
                    EnhancedLinkDB enhancedLinks = new EnhancedLinkDB();
                	SqlDataReader dr = enhancedLinks.GetSingleEnhancedLink(ItemID, WorkFlowVersion.Staging);
                
                    // Read in first row from database
					if (dr.Read())
					{
						TitleField.Text = (string) dr["Title"];
						DescriptionField.Text = (string) dr["Description"];
						UrlField.Text = (string) dr["Url"];
						Src.Text =(string) dr["ImageUrl"];
						MobileUrlField.Text = dr["MobileUrl"].ToString();
						ViewOrderField.Text = dr["ViewOrder"].ToString();
						CreatedBy.Text = (string) dr["CreatedByUser"];
						CreatedDate.Text = ((DateTime) dr["CreatedDate"]).ToShortDateString();
						TargetField.Items.FindByText((string) dr["Target"]).Selected = true;
						IsGroup.Checked = UrlField.Text.Equals ("SEPARATOR");
						if (UrlField.Text.Equals ("SEPARATOR")) 
						{
							oldUrl.Text = string.Empty;
						} 
						else 
						{
							oldUrl.Text = UrlField.Text;
						}
						estableceVisibilidad ();
					}
                    // Close datareader
                    dr.Close();
                }
            }
        }


		private void estableceVisibilidad ()
		{
			if (IsGroup.Checked) 
			{
				UrlField.Text = "SEPARATOR";
				UrlField.Visible = false;
				UrlFieldLabel.Visible = false;
				MobileUrlField.Visible = false;
				MobileUrlFieldLabel.Visible = false;
				TargetField.Visible = false;
				TargetFieldLabel.Visible = false;
			} 
			else 
			{
				UrlField.Text = oldUrl.Text;
				UrlField.Visible = true;
				UrlFieldLabel.Visible = true;
				MobileUrlField.Visible = true;
				MobileUrlFieldLabel.Visible = true;
				TargetField.Visible = true;
				TargetFieldLabel.Visible = true;
			}

		}

 		/// <summary>
		/// The UpdateBtn_Click event handler on this Page is used to either
		/// create or update a link.  It  uses the Rainbow.EnhancedLinkDB()
		/// data component to encapsulate all data functionality.
		/// </summary>
        override protected void OnUpdate(EventArgs e) 
        {
            base.OnUpdate(e);

            if (Page.IsValid == true) 
            {
                // Create an instance of the EnhancedLink DB component
                EnhancedLinkDB enhancedLinks = new EnhancedLinkDB();

                if (ItemID == 0) 
                {
                    // Add the link within the Links table
                    enhancedLinks.AddEnhancedLink(ModuleID, ItemID, PortalSettings.CurrentUser.Identity.Email, TitleField.Text, UrlField.Text, MobileUrlField.Text, Int32.Parse(ViewOrderField.Text), DescriptionField.Text, Src.Text, 0, TargetField.SelectedItem.Text);
                }
                else 
                {
                    // Update the link within the Links table
                    enhancedLinks.UpdateEnhancedLink(ModuleID, ItemID, PortalSettings.CurrentUser.Identity.Email, TitleField.Text, UrlField.Text, MobileUrlField.Text, Int32.Parse(ViewOrderField.Text), DescriptionField.Text, Src.Text, 0, TargetField.SelectedItem.Text);
                }

                // Redirect back to the portal home page
				this.RedirectBackToReferringPage();
			}
        }

		/// <summary>
		/// The DeleteBtn_Click event handler on this Page is used to delete 
		/// a link.  It  uses the Rainbow.EnhancedLinkDB()
		/// data component to encapsulate all data functionality.
		/// </summary>
        override protected void OnDelete(EventArgs e) 
        {
            base.OnDelete(e);

            // Only attempt to delete the item if it is an existing item
            // (new items will have "ItemID" of 0)
            if (ItemID != 0) 
            {
                EnhancedLinkDB enhancedLinks = new EnhancedLinkDB();
                enhancedLinks.DeleteEnhancedLink(ItemID);
            }

            // Redirect back to the portal home page
			this.RedirectBackToReferringPage();
		}

		#region Web Form Designer generated code
        /// <summary>
        /// Raises OnInitEvent
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() 
        {    
			this.IsGroup.CheckedChanged += new EventHandler(this.cmdIsGroup);
			this.Load += new EventHandler(this.Page_Load);

		}

		protected void cmdIsGroup(object sender, EventArgs e) 
		{
			if (IsGroup.Checked) 
			{
				UrlField.Text = "SEPARATOR";
			} 
			else 
			{
				UrlField.Text = oldUrl.Text;
			}
			estableceVisibilidad ();
		}

		#endregion
    }
}
