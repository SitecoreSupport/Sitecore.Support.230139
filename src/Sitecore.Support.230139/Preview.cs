using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Resources;
using Sitecore.Shell.DeviceSimulation;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.Framework.Scripts;
using Sitecore.Web.UI.Sheer;
using System;

namespace Sitecore.Shell.Framework.Commands.ContentEditor
{
  [Serializable]
  public class Preview : Command
  {
    public override void Execute(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      if (context.Items.Length == 1)
      {
        DeviceSimulationUtil.DeactivateSimulators();
        string formValue = WebUtil.GetFormValue("scEditorTabs");
        if (formValue.Contains("contenteditor:preview"))
        {
          SheerResponse.Eval("scContent.onEditorTabClick(null, null, 'Preview')");
        }
        else
        {
          UrlString urlString = new UrlString("/sitecore/shell/~/xaml/Sitecore.Shell.Applications.ContentEditor.Editors.Preview.aspx");
          context.Items[0].Uri.AddToUrlString(urlString);
          UIUtil.AddContentDatabaseParameter(urlString);
          ShowEditorTab showEditorTab = new ShowEditorTab();
          showEditorTab.Command = "contenteditor:preview";
          showEditorTab.Header = Translate.Text("Preview");
          showEditorTab.Icon = Images.GetThemedImageSource("Network/16x16/environment.png");
          showEditorTab.Url = urlString.ToString();
          showEditorTab.Id = "Preview";
          showEditorTab.Closeable = true;
          showEditorTab.Activate = true;
          ShowEditorTab showEditorTab2 = showEditorTab;
          SheerResponse.Eval(showEditorTab2.ToString());
        }
      }
    }

    public override CommandState QueryState(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      if (!Settings.Preview.Enabled)
      {
        return CommandState.Hidden;
      }
      if (context.Items.Length != 1)
      {
        return CommandState.Disabled;
      }
      Item item = context.Items[0];
      if (!base.HasField(item, FieldIDs.LayoutField))
      {
        return CommandState.Hidden;
      }
      return base.QueryState(context);
    }
  }
}