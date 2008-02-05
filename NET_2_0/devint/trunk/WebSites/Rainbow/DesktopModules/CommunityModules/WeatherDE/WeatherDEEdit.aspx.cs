using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rainbow.Framework.Providers;
using Rainbow.Framework.Web.UI;

namespace Rainbow.Content.Web.Modules
{
    /// <summary>
    /// WeatherDEEdit Module - Editpage part
    /// adapted from original version by: Mario Hartmann, Mario@Hartmann.net
    ///
    /// Original WeatherUSEdit Module
    /// Writen by: Jason Schaitel, Jason_Schaitel@hotmail.com
    /// Moved into Rainbow by Jakob Hansen, hansen3000@hotmail.com
    /// </summary>
    public partial class WeatherDEEdit : EditItemPage
    {
        protected TextBox Textbox2;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Construct the page
            // Added css Styles by Mario Endara <mario@softworks.com.uy> (2004/10/26)
            updateButton.CssClass = "CommandButton";
            PlaceHolderButtons.Controls.Add(updateButton);
            PlaceHolderButtons.Controls.Add(new LiteralControl("&#160;"));
            cancelButton.CssClass = "CommandButton";
            PlaceHolderButtons.Controls.Add(cancelButton);

            if (Page.IsPostBack == false)
            {
                WeatherZip.Text = "88045";
                WeatherCityIndex.Text = "0";
                WeatherSetting.SelectedIndex = 0;

                if (ModuleID > 0)
                {
                    if (ModuleSettings["WeatherZip"] != null)
                    {
                        WeatherZip.Text = ModuleSettings["WeatherZip"].ToString();
                    }

                    if (ModuleSettings["WeatherCityIndex"] != null)
                    {
                        WeatherCityIndex.Text = ModuleSettings["WeatherCityIndex"].ToString();
                    }

                    if (ModuleSettings["WeatherSetting"] != null)
                    {
                        WeatherSetting.SelectedIndex = int.Parse(ModuleSettings["WeatherSetting"].ToString());
                    }

                    if (ModuleSettings["WeatherDesign"] != null)
                    {
                        for (int i = 0; i < WeatherDesign.Items.Count; i++)
                        {
                            if (WeatherDesign.Items[i].Value == (ModuleSettings["WeatherDesign"].ToString()))
                            {
                                WeatherDesign.SelectedIndex = i;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set the module guids with free access to this page
        /// </summary>
        /// <value>The allowed modules.</value>
        protected override ArrayList AllowedModules
        {
            get
            {
                ArrayList al = new ArrayList();
                al.Add("D3182CD6-DAFF-4E72-AD9E-0B28CB44F000");
                return al;
            }
        }

        /// <summary>
        /// TBD
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);

            //only Update if the entered data is Valid
            if (Page.IsValid)
            {
                // UpProviderdate settings in the database
                RainbowModuleProvider.Instance.UpdateModuleSetting(ModuleID, "ProviderWeatherZip", WeatherZip.Text);
                RainbowModuleProvider.Instance.UpdateModuleSetting(ModuleID, "WeatherCityProviderIndex", WeatherCityIndex.Text);
                RainbowModuleProvider.Instance.UpdateModuleSetting(ModuleID, "WeatherSetting", WeatherSetting.Items[WeatherSetting.SelectedIndex].Value);
                RainbowModuleProvider.Instance.UpdateModuleSetting(ModuleID, "WeatherDesign", WeatherDesign.Items[WeatherDesign.SelectedIndex].Value);
                RedirectBackToReferringPage();
            }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInitEvent
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //Controls must be created here
            updateButton = new LinkButton();
            cancelButton = new LinkButton();

            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion
    }
}
