using System.Web;
using Esperantus.WebControls;
using Rainbow.Configuration;
using Rainbow.Settings;

namespace Rainbow.UI.WebControls
{
    /// <summary>
    /// HeaderImage
    /// </summary>
    public class HeaderImage : Image
    {
        /// <summary>
        /// DataBind
        /// </summary>
        override public void DataBind()
        {
            if(HttpContext.Current != null)
            {
                // Obtain PortalSettings from Current Context
            	PortalSettings portalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

                //PortalImage
                if(portalSettings.CustomSettings["SITESETTINGS_LOGO"] != null && portalSettings.CustomSettings["SITESETTINGS_LOGO"].ToString().Length != 0)
                {
                    this.ImageUrl = Path.WebPathCombine(Path.ApplicationRoot, portalSettings.PortalPath, portalSettings.CustomSettings["SITESETTINGS_LOGO"].ToString());
					// Added by Mario Endara to Reinforce portal Title for Search Engines <mario@softworks.com.uy>
					if(portalSettings.CustomSettings["SITESETTINGS_PAGE_TITLE"] != null && 
						portalSettings.CustomSettings["SITESETTINGS_PAGE_TITLE"].ToString().Length != 0)
						this.AlternateText = portalSettings.CustomSettings["SITESETTINGS_PAGE_TITLE"].ToString();
                    this.Visible = true;
                }
                else
                {
                    this.Visible = false;
                }
            }
            base.DataBind();
        }    
    }
}