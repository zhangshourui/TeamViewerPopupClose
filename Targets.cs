using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamViewerPopupClose
{

    /// <summary>
    /// target window & child window detect configuration
    /// </summary>
    public class TargetConfig
    {
        /// <summary>
        /// If the window is match any of the targets. it will be closed.
        /// </summary>
        public Target[] Targets { get; set; }
    }

    public class Target
    {
        /// <summary>
        /// Window title text. if empty, then match all windows and ignore <see cref="TitleContains"/>; 
        /// </summary>
        public string WindowTitle { get; set; }

        /// <summary>
        /// true when the window title contains the string of  <see cref="WindowTitle"/>; 
        /// false when the window title exactly match the string of   <see cref="WindowTitle"/>。
        /// </summary>
        public bool TitleContains { get; set; }

        /// <summary>
        /// The child window text. Usually a button or a label text. <c>ChildText</c> must exactly matches the window text.
        /// </summary>
        public string ChildText { get; set; }
    }

}
