using System.Collections.Generic;

namespace Wox.Plugin.Devbox.Helpers
{
  class ApiResult
  {
    public int total_count { get; set; }
    public bool incomplete_results { get; set; }
    public List<ApiResultRepo> items { get; set; }
  }
}
