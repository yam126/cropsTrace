#pragma checksum "E:\workproject\cropsTrace\cropsTrace\Views\Shared\_LayoutList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8b1e39582061e35d44550c44978969fcc50fa81d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__LayoutList), @"mvc.1.0.view", @"/Views/Shared/_LayoutList.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8b1e39582061e35d44550c44978969fcc50fa81d", @"/Views/Shared/_LayoutList.cshtml")]
    #nullable restore
    public class Views_Shared__LayoutList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("menuIcon"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/menuIcon.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "E:\workproject\cropsTrace\cropsTrace\Views\Shared\_LayoutList.cshtml"
  
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<!--管理菜单页面-->
<el-container id=""managerMenu"" class=""elContainer"">
    <div class=""left-container"">
        <div style=""height:32%"" class=""content-container"">
            <!--管理菜单-->
            <div class=""title-container"">
                <div class=""title-text"">
                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "8b1e39582061e35d44550c44978969fcc50fa81d3837", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("快捷菜单\r\n                </div>\r\n            </div>\r\n            <div class=\"middle-container\">\r\n                <div id=\"adminMenu\">\r\n                    <!--菜单项目-->\r\n                    <div ");
            WriteLiteral(@"@click=""gotoPage('/Home/Index')"" class=""menuItem-normal"">
                        <div class=""top-left-corner""></div>
                        <div class=""top-right-corner""></div>
                        <div class=""bottom-left-corner""></div>
                        <div class=""bottom-right-corner""></div>
                        <div class=""center-container"">
                            返回首页
                        </div>
                    </div>
                    <!--/菜单项目-->
                    <!--菜单项目-->
                    <div ");
            WriteLiteral(@"@click=""gotoPage('/GrowthInfo/GrowthInfo')"" :class=""currentAction=='GrowthInfo'?'menuItem-click':'menuItem-normal'"">
                        <div class=""top-left-corner""></div>
                        <div class=""top-right-corner""></div>
                        <div class=""bottom-left-corner""></div>
                        <div class=""bottom-right-corner""></div>
                        <div class=""center-container"">
                            生长信息
                        </div>
                    </div>
                    <!--/菜单项目-->
                    <!--菜单项目-->
                    <div ");
            WriteLiteral(@"@click=""gotoPage('/SeedInfo/SeedInfo')"" :class=""currentAction=='SeedInfo'?'menuItem-click':'menuItem-normal'"">
                        <div class=""top-left-corner""></div>
                        <div class=""top-right-corner""></div>
                        <div class=""bottom-left-corner""></div>
                        <div class=""bottom-right-corner""></div>
                        <div class=""center-container"">
                            农作物主数据
                        </div>
                    </div>
                    <!--/菜单项目-->
                </div>
            </div>
            <!--底部层-->
            <div class=""bottom-container""></div>
            <!--/底部层-->
            <!--/管理菜单-->
        </div>
    </div>
    <div id=""adminPage"">
        ");
#nullable restore
#line 58 "E:\workproject\cropsTrace\cropsTrace\Views\Shared\_LayoutList.cshtml"
   Write(RenderBody());

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n</el-container>\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
