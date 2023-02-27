using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace luisvaldez.EditableContentControl
{
  public class ContentElement
  {
    public ContentElement()
    {
      LastMofidiedDateTime = DateTime.UtcNow;
    }
    public string SourceID { get; set; }
    public string URL { get; set; }
    public DateTime LastMofidiedDateTime { get; set; }
    public string HTML { get; set; }
    public string PlainText
    {
      get
      {
        if (string.IsNullOrEmpty(HTML))
          return string.Empty;
        return Regex.Replace(HTML, "<.*?>", "");
      }
    }
    public string Language { get; set; }
    public string Version { get; internal set; }
  }
  public enum EditorMode
  {
    HTML = 0,
    PlainText = 1
  }

  public partial class EditableUserContent : UserControl
  {
    public int MinLength { get; set; }
    public int MaxLength { get; set; }
    public bool Required { get; set; }
    public List<Func<string, string>> ReplacementMethods { get; set; }
    public string DefaultContent { get; set; }
    public delegate ContentElement PreRenderEventHandler(ContentElement contentElement);
    public event PreRenderEventHandler PreRenderEvent;

    public EditorMode EditMode { get; set; }
    
    public string SourceID { get; set; }
    private static HashSet<string> defaultDocuments = new HashSet<string> { "default.aspx" };
    public string URL { get; set; }
    public ContentElement ContentElement { get; set; }

    public delegate ContentElement ContentRequestEventHandler(string url, string sourceID, string language);
    public ContentRequestEventHandler OnContentRequest { get; set; }

    public string Language { get; set; }

    public EditableUserContent()
    {
      ReplacementMethods = new List<Func<string, string>>();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(URL))
      {
        URL = HttpContext.Current.Request.RawUrl;// HttpContext.Current.Request.GetCurrentUrl().AbsolutePath;
        URL = Regex.Replace(URL, "/default.aspx", "/", RegexOptions.IgnoreCase);
      }
      if (string.IsNullOrEmpty(Language))
        Language = "en";
      ContentElement = OnContentRequest(URL, SourceID, Language);
      if (ContentElement == null)
      {
        ContentElement = new ContentElement();
        ContentElement.URL = URL;
        ContentElement.SourceID = SourceID;
        ContentElement.Language = Language;
        ContentElement.Version = BitConverter.ToString(new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 }).Replace("-", "");
        ContentElement.HTML = DefaultContent;
      }
      else
      {
        if (PreRenderEvent != null)
          ContentElement = PreRenderEvent(ContentElement);
      }
      adminControls.Attributes.Add("data-url", URL);
      adminControls.Attributes.Add("data-language", Language);
      adminControls.Attributes.Add("data-sourceid", ContentElement.SourceID);
      adminControls.Attributes.Add("data-version", ContentElement.Version);

      SaveButton.Attributes.Add("data-url", URL);
      SaveButton.Attributes.Add("data-language", Language);
      SaveButton.Attributes.Add("data-sourceid", ContentElement.SourceID);
      SaveButton.Attributes.Add("data-version", ContentElement.Version);
    }
  }
}