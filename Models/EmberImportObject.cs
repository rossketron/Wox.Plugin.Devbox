namespace Wox.Plugin.Devbox.Helpers
{
  class EmberImportObject
  {
    public string global { get; set; }
    public string module { get; set; }
    public string export { get; set; }
    public string localName { get; set; }
    public bool deprecated { get; set; }
    public DeprecatedReplacement replacement { get; set; }
  }
}
