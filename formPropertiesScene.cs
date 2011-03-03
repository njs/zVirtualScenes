﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zVirtualScenesApplication
{
    public partial class formPropertiesScene : Form
    {
        public bool isOpen { get; set; }
        public formzVirtualScenes _zVirtualScenesMain;
        public int _SelectedSceneIndex;     

        public formPropertiesScene()
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formSceneProperties_Close);
            this.isOpen = false;            
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Scene selectedscene = _zVirtualScenesMain.MasterScenes[_SelectedSceneIndex];
            //HANDLE Scene NAME CHANGE
            if (txtb_sceneName.Text != "")            
                selectedscene.Name = txtb_sceneName.Text;            
            else
            {
                MessageBox.Show("Invalid scene name.", _zVirtualScenesMain.ProgramName);
                return;
            }

            //Global Hotkey
            selectedscene.GlobalHotKey = (int)Enum.Parse(typeof(zVirtualScenesApplication.formzVirtualScenes.CustomHotKeys), comboBoxHotKeys.SelectedValue.ToString());

            this.Close();
        }

        public void SetGlobalHotKey(string HotKeyString)
        {
            comboBoxHotKeys.SelectedIndex = (int)Enum.Parse(typeof(zVirtualScenesApplication.formzVirtualScenes.CustomHotKeys), HotKeyString);
        }

        private void formSceneProperties_Close(object sender, EventArgs e)
        {
            this.isOpen = false;
        }

        private void formSceneProperties_Load(object sender, EventArgs e)
        {
            this.isOpen = true;

            txtb_sceneName.Text = _zVirtualScenesMain.MasterScenes[_SelectedSceneIndex].Name;
            comboBoxHotKeys.DataSource = Enum.GetNames(typeof(zVirtualScenesApplication.formzVirtualScenes.CustomHotKeys));
            comboBoxHotKeys.SelectedIndex = _zVirtualScenesMain.MasterScenes[_SelectedSceneIndex].GlobalHotKey;
        }
    }       
}