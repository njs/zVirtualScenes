//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace zvsModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    
    public partial class plugin
    {
        public plugin()
        {
            this.device_types = new ObservableCollection<device_types>();
            this.plugin_settings = new ObservableCollection<plugin_settings>();
        }
    
        public int id { get; set; }
        public string friendly_name { get; set; }
        public string name { get; set; }
        public bool enabled { get; set; }
        public string description { get; set; }
    
        public virtual ObservableCollection<device_types> device_types { get; set; }
        public virtual ObservableCollection<plugin_settings> plugin_settings { get; set; }
    }
}