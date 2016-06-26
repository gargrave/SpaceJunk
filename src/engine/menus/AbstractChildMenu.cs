using UnityEngine;
using System.Collections;

namespace gkh
{
    public abstract class AbstractChildMenu : AbstractMenu
    {
        #region
        protected IParentMenu parentScreen;
        #endregion


        #region child menu methods
        public virtual bool ParentIsSet() { return parentScreen != null; }

        public virtual void SetParentMenu(IParentMenu parent)
        { this.parentScreen = parent; }
        #endregion
    }
}