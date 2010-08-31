namespace Rainbow.Framework.Services
{
    using System;
    using System.Collections;

    /// <summary>
    /// Lists the possible types of services that can be used
    ///     with a community.
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// The unknown.
        /// </summary>
        Unknown = 0, 

        /// <summary>
        /// The community web service.
        /// </summary>
        CommunityWebService = 1, 

        /// <summary>
        /// The community rss service.
        /// </summary>
        CommunityRSSService = 2, 

        /// <summary>
        /// The rss service.
        /// </summary>
        RSSService = 3
    }

    /// <summary>
    /// Lists the possible types of services
    /// </summary>
    public enum ServiceListType
    {
        /// <summary>
        /// The item type.
        /// </summary>
        Item = 1, 

        /// <summary>
        /// The module.
        /// </summary>
        Module = 2, 

        /// <summary>
        /// The tab / page.
        /// </summary>
        Tab = 3
    }

    /// <summary>
    /// This class Represents all the information about what the requested
    ///     service (described in the URL attribute) should return.
    /// </summary>
    public class ServiceRequestInfo
    {
        #region Constants and Fields

        // Aka. field [rb_GeneralModuleDefinitions].[ClassName]. "All" is all classes supported by search

        // All;Announcements;Contacts;Discussion;Events;HtmlModule;Documents;Pictures;Articles;Tasks;FAQs;ComponentModule

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRequestInfo"/> class.
        /// </summary>
        public ServiceRequestInfo()
        {
            this.IDList = string.Empty;
            this.IDListType = ServiceListType.Tab;
            this.ListType = ServiceListType.Tab;
            this.LocalMode = true;
            this.MaxHits = 20;
            this.ModuleType = "All";
            this.PortalAlias = string.Empty;
            this.SearchField = string.Empty;
            this.SearchString = string.Empty;
            this.SortDirection = "ASC";
            this.SortField = "ModuleName";
            this.Type = ServiceType.Unknown;
            this.Url = string.Empty;
            this.UserName = string.Empty;
            this.UserPassword = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Comma separated list of ID's. e.g.: 1234,234,5454.
        ///     Only data for these ID's are listed. The ID type is controlled using
        ///     attribute IDListType
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The ID list.</value>
        public string IDList { get; set; }

        /// <summary>
        ///     Controls the type of ID's in attribute IDList
        ///     Default value: ServiceListType.Tab
        /// </summary>
        /// <value>The type of the ID list.</value>
        public ServiceListType IDListType { get; set; }

        /// <summary>
        ///     The type of the required list. See enum ServiceListType
        ///     Default value: ServiceListType.Tab
        /// </summary>
        /// <value>The type of the list.</value>
        public ServiceListType ListType { get; set; }

        /// <summary>
        ///     When true the Url is ignored and the local Rainbow site is used as data soure
        ///     Default value: false
        /// </summary>
        /// <value><c>true</c> if [local mode]; otherwise, <c>false</c>.</value>
        public bool LocalMode { get; set; }

        /// <summary>
        ///     Represents the number of items returned by the service.
        ///     Default value: 20
        /// </summary>
        /// <value>The max hits.</value>
        public int MaxHits { get; set; }

        /// <summary>
        ///     If true only data for mobile devices are listed
        ///     Default value: false
        /// </summary>
        /// <value><c>true</c> if [mobile only]; otherwise, <c>false</c>.</value>
        public bool MobileOnly { get; set; }

        /// <summary>
        ///     Module type
        ///     See database: [rb_GeneralModuleDefinitions].[ClassName]
        ///     Valid values: All;Announcements;Contacts;Discussion;Events;HtmlModule;Documents;Pictures;Articles;Tasks;FAQs;ComponentModule
        ///     Default value: "All".
        /// </summary>
        /// <value>The type of the module.</value>
        public string ModuleType { get; set; }

        /// <summary>
        ///     Represents the Alias of the site
        ///     See database: [rb_Portal].[PortalAlias]
        ///     Default value: string.Empty (the value of web.config key DefaultPortal will be used!)
        /// </summary>
        /// <value>The portal alias.</value>
        public string PortalAlias { get; set; }

        /// <summary>
        ///     If true only tabs or modules where tab parent is at top level are listed
        ///     Default value: false
        /// </summary>
        /// <value><c>true</c> if [root level only]; otherwise, <c>false</c>.</value>
        public bool RootLevelOnly { get; set; }

        /// <summary>
        ///     Set this if only a single field should be searched e.g.: "Title"
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The search field.</value>
        public string SearchField { get; set; }

        /// <summary>
        ///     Search string. Note: different behaviour depending on the ListType
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The search string.</value>
        public string SearchString { get; set; }

        /// <summary>
        ///     If true ID's are displyed in the lists
        ///     Default value: false
        /// </summary>
        /// <value><c>true</c> if [show ID]; otherwise, <c>false</c>.</value>
        public bool ShowID { get; set; }

        /// <summary>
        ///     Sort Ascending or Descending
        ///     Valid values: ASC;DESC
        ///     Default value: ASC
        /// </summary>
        /// <value>The sort direction.</value>
        public string SortDirection { get; set; }

        /// <summary>
        ///     Sort list on this field
        ///     Valid values: ModuleName;Title;CreatedByUser;CreatedDate;TabName
        ///     Default value: ModuleName
        /// </summary>
        /// <value>The sort field.</value>
        public string SortField { get; set; }

        /// <summary>
        ///     The service that receives this tag does a check and see if it can
        ///     do the special thingy that the tag value controls.
        ///     If the tag value is not supported "Tag=X not supported" is returned
        ///     in field ServiceResponseInfo.ServiceStatus. The service should try
        ///     to deliver data for "normal case" (Tag=0).
        /// </summary>
        /// <value>The tag.</value>
        public int Tag { get; set; }

        /// <summary>
        ///     Represents the type of the service such as community Web
        ///     service or RSS.
        ///     Default value: ServiceType.Unknown
        /// </summary>
        /// <value>The type.</value>
        public ServiceType Type { get; set; }

        /// <summary>
        ///     Represents the URL of the service (aka. data source)
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        ///     Used together with UserPassword to sign-in and retrieve data with this
        ///     users credentials/rights
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        ///     Used together with UserName to sign-in and retrieve data with this
        ///     users credentials/rights
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The user password.</value>
        public string UserPassword { get; set; }

        #endregion
    }

    /// <summary>
    /// This class Represents the response from a service including
    ///     the response from a community Web service and a RSS service.
    /// </summary>
    public class ServiceResponseInfo
    {
        #region Constants and Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponseInfo"/> class.
        /// </summary>
        public ServiceResponseInfo()
        {
            this.Items = new ArrayList();
            this.ServiceCopyright = string.Empty;
            this.ServiceDescription = string.Empty;
            this.ServiceImageLink = string.Empty;
            this.ServiceImageTitle = string.Empty;
            this.ServiceImageUrl = string.Empty;
            this.ServiceLink = string.Empty;
            this.ServiceStatus = "Unknown";
            this.ServiceTitle = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Represents the content items returned by the service.
        /// </summary>
        /// <value>The items.</value>
        public ArrayList Items { get; set; }

        /// <summary>
        ///     Represents copyright information associated with the service.
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The service copyright.</value>
        public string ServiceCopyright { get; set; }

        /// <summary>
        ///     Represents the description of the service.
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The service description.</value>
        public string ServiceDescription { get; set; }

        /// <summary>
        ///     Represents a URL associated with a service image.
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The service image link.</value>
        public string ServiceImageLink { get; set; }

        /// <summary>
        ///     Represents the text associated with the service image.
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The service image title.</value>
        public string ServiceImageTitle { get; set; }

        /// <summary>
        ///     Represents the URL of an image associated with the service.
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The service image URL.</value>
        public string ServiceImageUrl { get; set; }

        /// <summary>
        ///     Represents the URL of the service.
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The service link.</value>
        public string ServiceLink { get; set; }

        /// <summary>
        ///     Contains a response code from the service. If everything
        ///     works, the response is "OK".
        ///     Default value: the string "Unknown"
        /// </summary>
        /// <value>The service status.</value>
        public string ServiceStatus { get; set; }

        /// <summary>
        ///     Represents the title of the service.
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The service title.</value>
        public string ServiceTitle { get; set; }

        #endregion
    }

    /// <summary>
    /// This class represents a particular content item returned by a service.
    /// </summary>
    public class ServiceResponseInfoItem
    {
        // RSS NOTE: RSS only uses attributes Link, Title and Description
        #region Constants and Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponseInfoItem"/> class.
        /// </summary>
        public ServiceResponseInfoItem()
        {
            this.CreatedByUser = string.Empty;
            this.Description = string.Empty;
            this.FriendlyName = string.Empty;
            this.GeneralModDefID = string.Empty;
            this.ItemID = -1;
            this.Link = string.Empty;
            this.ModuleID = -1;
            this.ModuleTitle = string.Empty;
            this.PageID = -1;
            this.PageName = string.Empty;
            this.Title = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Name (email) of the user that created the item
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The created by user.</value>
        public string CreatedByUser { get; set; }

        /// <summary>
        ///     Creation date of the item
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     Title of the item/module/tab (aka Abstract)
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        ///     Name of module in english
        ///     See database: [rb_GeneralModuleDefinitions].[FriendlyName]
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The name of the friendly.</value>
        public string FriendlyName { get; set; }

        /// <summary>
        ///     GUID ID of module.
        ///     See database: [rb_GeneralModuleDefinitions].[GeneralModDefID]
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The general mod def ID.</value>
        public string GeneralModDefID { get; set; }

        /// <summary>
        ///     Item ID
        ///     Default value: -1
        /// </summary>
        /// <value>The item ID.</value>
        public int ItemID { get; set; }

        /// <summary>
        ///     URL link that later is applyed to the attribute Title
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The link.</value>
        public string Link { get; set; }

        /// <summary>
        ///     The module ID of the item
        ///     Default value: -1
        /// </summary>
        /// <value>The module ID.</value>
        public int ModuleID { get; set; }

        /// <summary>
        ///     Name of module where the item is placed
        ///     See database: [rb_Modules].[ModuleTitle]
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The module title.</value>
        public string ModuleTitle { get; set; }

        /// <summary>
        ///     The Tab ID of the items module
        ///     Default value: -1
        /// </summary>
        /// <value>The page ID.</value>
        public int PageID { get; set; }

        /// <summary>
        ///     Name of tab where the item is displayed
        ///     See database: [rb_Tabs].[TabName]
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The name of the page.</value>
        public string PageName { get; set; }

        /// <summary>
        ///     Title of the item/module/tab
        ///     Default value: string.Empty
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        #endregion
    }
}