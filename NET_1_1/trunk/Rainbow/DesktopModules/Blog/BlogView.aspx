<%@ Page language="c#" CodeBehind="BlogView.aspx.cs" AutoEventWireup="false" Inherits="Rainbow.DesktopModules.BlogView" %>
<%@ Register TagPrefix="foot" TagName="Footer" Src="~/Design/DesktopLayouts/DesktopFooter.ascx" %>
<%@ Register TagPrefix="portal" TagName="Banner" Src="~/Design/DesktopLayouts/DesktopPortalBanner.ascx" %>
<%@ Register TagPrefix="tra" Namespace="Esperantus.WebControls" Assembly="Esperantus" %>
<html>
	<head runat="server"></head>
	<body runat="server">
		<form id="BlogView" method="post" runat="server">
			<div class="zen-main" id="zenpanes">
				<div class="rb_DefaultPortalHeader">
					<portal:banner id="SiteHeader" runat="server"></portal:banner>
				</div>
				<div class="div_ev_Table">
					<div class="div_blog">
						<div class="div_blog_stats">
							<b><tra:Literal id="SyndicationLabel" runat="server" Text="Syndication" TextKey="BLOG_SYNDICATION"></tra:Literal></b><br />
							<a id="lnkRSS" runat="server" href="/DesktopModules/Blog/rss.aspx"><img id="imgRSS" runat="server" src="/rainbow/DesktopModules/Blog/xml.gif" border="0" /></a>
							<br /><br />
							<b><tra:Literal id="StatisticsLabel" runat="server" Text="Statistics" TextKey="BLOG_STATISTICS"></tra:Literal></b><br />
							<asp:Label ID="lblEntryCount" Runat="server"></asp:Label><br />
							<asp:Label ID="lblCommentCount" Runat="server"></asp:Label><br />
							<br />
							<b><tra:Literal id="ArchivesLabel" runat="server" Text="Archives" TextKey="BLOG_ARCHIVES"></tra:Literal></b><br />
							<asp:Repeater id="dlArchive" EnableViewState="False" runat="server">
								<HeaderTemplate>
									<ul>
								</HeaderTemplate>
								<ItemTemplate><li>
									<asp:HyperLink id="Hyperlink4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MonthName") 
																+ ", " +  DataBinder.Eval(Container.DataItem,"Year")
																+ " (" + DataBinder.Eval(Container.DataItem,"Count") + ")"%>' Visible='True' NavigateUrl='<%# Rainbow.HttpUrlBuilder.BuildUrl("~/DesktopModules/Blog/ArchiveView.aspx",PageID,
									"&month=" + DataBinder.Eval(Container.DataItem,"Month") 
									+ "&year=" + DataBinder.Eval(Container.DataItem,"Year")
									+ "&mid=" + ModuleID )%>'>
									</asp:HyperLink>&nbsp;</li>
								</ItemTemplate>
								<FooterTemplate>
									</ul>
								</FooterTemplate>
							</asp:Repeater>
						</div>
						<div class="div_blog_messeges">
							<table cellSpacing="0" cellPadding="0" width="100%">
								<tr>
									<td colSpan="2">
										<P><asp:Label id="Title" runat="server" CssClass="BlogTitle">Title</asp:Label>&nbsp;</P>
										<P><asp:Label id="Description" runat="server" CssClass="Normal">Description</asp:Label></P>
										<P><asp:Label id="StartDate" runat="server" CssClass="ItemDate">StartDate</asp:Label></P>
										<hr noshade size="1" />
									</td>
								</tr>
							</table>
						</div>
						<div class="div_blog_comments">
							<b><tra:Literal id="Literal6" runat="server" TextKey="BLOG_TITLE_FEEDBACK" Text="Feedback"></tra:Literal></b>
							<asp:datalist id="dlComments" OnItemCommand="dlComments_ItemCommand" Width="100%" CellPadding="4"
								EnableViewState="False" runat="server">
								<ItemTemplate>
										<div>
											<div class="BlogTitle">
												<tra:ImageButton TextKey="DELETE" Text="Delete" id="btnDelete" ImageUrl='<%# CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl %>' CommandName="DeleteComment" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"BlogCommentID")%>' Visible="<% # IsDeleteable %>" runat="server" />
												<%# DataBinder.Eval(Container.DataItem,"Title") %>
												</asp:HyperLink>&#160;
											</div>
											<br />
											<asp:Label id="Label3" runat="server" CssClass="BlogCommentName" Visible='<%# (bool) (DataBinder.Eval(Container.DataItem, "URL").ToString().Length == 0) %>'>
												<%# Server.HtmlDecode((String) DataBinder.Eval(Container.DataItem,"Name")) %>
											</asp:Label>
											<asp:HyperLink id="Hyperlink2" Visible='<%# (bool) (DataBinder.Eval(Container.DataItem, "URL").ToString().Length != 0) %>' runat="server" Target="_blank" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>' CssClass="BlogCommentName" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"URL")%>'>
											</asp:HyperLink>&#160;
											<br />
											<asp:Label id="Label2" Visible="True" runat="server" CssClass="BlogItemDate" Text='<%# DataBinder.Eval(Container.DataItem,"DateCreated", "{0:dddd MMMM d yyyy h:mm tt}") %>'/>
											<br />
											<%# Server.HtmlDecode((String) DataBinder.Eval(Container.DataItem,"Comment")) %>
											<br />
											<br />
											<div class="BlogFooter">
											</div>
										</div>
									<hr />
								</ItemTemplate>
							</asp:datalist>
						</div>
						<div class="div_blog_comment_form">
							<!-- begin comments form -->
							<table border="0">
								<tr>
									<td align="left">
										<tra:Literal id="Literal4" runat="server" TextKey="BLOG_TITLE" Text="Title"></tra:Literal>&nbsp;
									</td>
									<td>
										<asp:TextBox ID="txtTitle" Runat="server" MaxLength="100" Width="300"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td align="left">
										<tra:Literal id="Literal1" runat="server" TextKey="BLOG_NAME" Text="Name"></tra:Literal>&nbsp;
									</td>
									<td>
										<asp:TextBox ID="txtName" Runat="server" MaxLength="100" Width="300"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td align="left">
										<tra:Literal id="Literal2" runat="server" TextKey="BLOG_URL" Text="URL"></tra:Literal>&nbsp;
									</td>
									<td>
										<asp:TextBox ID="txtURL" Runat="server" MaxLength="200" Width="300"></asp:TextBox>
									</td>
								</tr>
							</table>
							<tra:Literal id="Literal3" runat="server" TextKey="BLOG_COMMENTS" Text="Comments"></tra:Literal>
							<br />
							<asp:TextBox ID="txtComments" Runat="server" TextMode="MultiLine" Width="400" Height="200"></asp:TextBox>
							<br />
							<asp:CheckBox ID="chkRememberMe" Runat="server"></asp:CheckBox>&nbsp;
							<tra:Literal id="Literal5" runat="server" TextKey="BLOG_REMEMBER_ME" Text="Remember Me?"></tra:Literal>
							<br />
							<tra:Button id="btnPostComment" runat="server" TextKey="SUBMIT" Text="Submit"></tra:Button>
							<!-- end comments form -->
						</td>
					</div>
					<div class="copyright">
							<asp:Label ID="lblCopyright" Runat="server" CssClass="Normal"></asp:Label>
					</div>
				</div>
			</div>
				<div class="rb_AlternatePortalFooter"><foot:Footer id="Footer" runat="server"></foot:Footer></div></foot:Footer>
			</div>
		</form>
	</body>
</html>
