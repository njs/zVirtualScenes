﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ControlThink.ZWave.Devices;

namespace zVirtualScenesApplication
{
    class ControlThinkRefresh
    {
        formzVirtualScenes zVirtualScenesMain;
        public delegate void DeviceInfoChangeEventHandler(string GlbUniqueID, string TypeOfChange);
        public event DeviceInfoChangeEventHandler DeviceInfoChange;

        public ControlThinkRefresh(formzVirtualScenes _zVirtualScenesMain)
        {
            this.zVirtualScenesMain = _zVirtualScenesMain;
        }

        /// <summary>
        /// This continually looks for changes in device modes.  When one is detected, it will change MasterDeviceList and call the DeviceInfoChangeEventHandler. 
        /// </summary>
        public void RefreshThread()
        {
            while (true)
            {
                if (zVirtualScenesMain.ControlThinkController.IsConnected)
                {
                    //For each device on Control Stick 
                    foreach (ZWaveDevice device in zVirtualScenesMain.ControlThinkController.Devices)
                    {
                        try
                        {
                            //If device type on Control Stick is allowed
                            if (!device.ToString().Contains("Controller")) //Do not include ZWave controllers for now...
                            {
                                //get type
                                string devicetype = device.ToString().Replace("ControlThink.ZWave.Devices.Specific.", "");

                                //for each device previously saved in memory
                                foreach (Device thisDevice in zVirtualScenesMain.MasterDevices)  
                                {
                                    //if Control Stick device == device in memory
                                    if (zVirtualScenesMain.ControlThinkController.HomeID.ToString() + device.NodeID.ToString() == thisDevice.GlbUniqueID())
                                    {
                                        #region DETECT LEVEL CHANGES IN ALL DEVICES
                                        //Check to see if any device state/level has changed.
                                        if (device.Level != thisDevice.Level)
                                        {
                                            thisDevice.prevLevel = thisDevice.Level;
                                            thisDevice.Level = device.Level; //set MasterDeviceList
                                            this.DeviceInfoChange(thisDevice.GlbUniqueID(), "level"); //call event                                                
                                        }
                                        #endregion

                                        #region DETECT THERMOSTAT SPECIFIC CHANGES
                                        if (devicetype.Contains("GeneralThermostat"))
                                        {
                                            ControlThink.ZWave.Devices.Specific.GeneralThermostatV2 thermostat = (ControlThink.ZWave.Devices.Specific.GeneralThermostatV2)device;

                                            int coolpoint = (int)thermostat.ThermostatSetpoints[ThermostatSetpointType.Cooling1].Temperature.ToFahrenheit();
                                            //If ThermostatSetpoints[x] returns 0 C and gets converted to 32 F recheck to make sure it is the intended figure. 
                                            //Either the ControlThink Stick or certian thermostats falsely return Heat and Cool points of 32 F, upon a 
                                            //second query they return the proper value therefor we will requery if we initially get 32
                                            if (coolpoint == 32)
                                            {
                                                Thread.Sleep(200);
                                                coolpoint = (int)thermostat.ThermostatSetpoints[ThermostatSetpointType.Cooling1].Temperature.ToFahrenheit();
                                            }
                                            
                                            int heatpoint = (int)thermostat.ThermostatSetpoints[ThermostatSetpointType.Heating1].Temperature.ToFahrenheit();
                                            if (heatpoint == 32)
                                            {
                                                Thread.Sleep(200);
                                                heatpoint = (int)thermostat.ThermostatSetpoints[ThermostatSetpointType.Heating1].Temperature.ToFahrenheit();
                                            }

                                            int currenttemp = (int)thermostat.ThermostatTemperature.ToFahrenheit();
                                            int fanmode = (int)thermostat.ThermostatFanMode;
                                            int mode = (int)thermostat.ThermostatMode;
                                            byte level = thermostat.Level;
                                            string currentstate = thermostat.ThermostatOperatingState.ToString() + "-" + thermostat.ThermostatFanMode.ToString();
                                                                                        

                                            if (thisDevice.Temp != currenttemp)
                                            {
                                                thisDevice.prevTemp = thisDevice.Temp; //Save old temp
                                                thisDevice.Temp = currenttemp; //Save new Temp
                                                this.DeviceInfoChange(thisDevice.GlbUniqueID(), "Temp"); //call event 
                                            }

                                            if (thisDevice.CoolPoint != coolpoint)
                                            {
                                                thisDevice.prevCoolPoint = thisDevice.CoolPoint; 
                                                thisDevice.CoolPoint = coolpoint;
                                                this.DeviceInfoChange(thisDevice.GlbUniqueID(), "CoolPoint"); //call event
                                            }

                                            if (thisDevice.HeatPoint != heatpoint)
                                            {
                                                thisDevice.prevHeatPoint = thisDevice.HeatPoint; 
                                                thisDevice.HeatPoint = heatpoint;
                                                this.DeviceInfoChange(thisDevice.GlbUniqueID(), "HeatPoint"); //call event
                                            }

                                            if (thisDevice.FanMode != fanmode)
                                            {
                                                thisDevice.prevFanMode = thisDevice.FanMode;
                                                thisDevice.FanMode = fanmode;
                                                this.DeviceInfoChange(thisDevice.GlbUniqueID(), "FanMode"); //call event
                                            }

                                            if (thisDevice.HeatCoolMode != mode)
                                            {
                                                thisDevice.prevHeatCoolMode = thisDevice.HeatCoolMode; 
                                                thisDevice.HeatCoolMode = mode;
                                                this.DeviceInfoChange(thisDevice.GlbUniqueID(), "HeatCoolMode"); //call event
                                            }                                            

                                            if (thisDevice.CurrentState != currentstate)
                                            {
                                                thisDevice.prevCurrentState = thisDevice.CurrentState; 
                                                thisDevice.CurrentState = currentstate;
                                                this.DeviceInfoChange(thisDevice.GlbUniqueID(), "CurrentState"); //call event
                                            }
                                        }
#endregion
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            zVirtualScenesMain.LogThis(2, "ControlThink USB Trouble Getting Device Status: " + ex.Message);
                        }
                    }
                }
                else
                    zVirtualScenesMain.ControlThinkConnect();
                Thread.Sleep(10000);
            }          

        }
    }
}
