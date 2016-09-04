// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Dev;
using System.Dynamic;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(false)]
    public class AraGridControlPanel:AraDiv
    {

        public enum ESide
        {
            left, right, top, bottom
        }

        private ESide _Side;

        private AraObjectInstance<AraGrid> _Grid = new AraObjectInstance<AraGrid>();
        public AraGrid Grid
        {
            get { return _Grid.Object; }
            set { _Grid.Object = value; }
        }

        private AraObjectInstance<AraDiv> _ControlPanelDiv = new AraObjectInstance<AraDiv>();
        public AraDiv ControlPanelDiv
        {
            get { return _ControlPanelDiv.Object; }
            set { _ControlPanelDiv.Object = value; }
        }

        private AraObjectInstance<AraTable> _ControlPanelTable = new AraObjectInstance<AraTable>();
        public AraTable ControlPanelTable
        {
            get { return _ControlPanelTable.Object; }
            set { _ControlPanelTable.Object = value; }
        }

        private AraObjectInstance<AraTableTr> _ControlPanelTableTr = new AraObjectInstance<AraTableTr>();
        public AraTableTr ControlPanelTableTr
        {
            get { return _ControlPanelTableTr.Object; }
            set { _ControlPanelTableTr.Object = value; }
        }

        public AraGridControlPanel(IAraObjectClienteServer vConteiner) :
            this(vConteiner,ESide.bottom)
        {
        }

        public AraGridControlPanel(IAraObjectClienteServer vConteiner, ESide vSide) :
            base((IAraContainerClient)vConteiner)
        {
            Grid = new AraGrid(this);
            ControlPanelDiv = new AraDiv(this);
            ControlPanelDiv.ScrollBar = new AraScrollBar(ControlPanelDiv);
            
                        

            ControlPanelTable = new AraTable(ControlPanelDiv);
            ControlPanelTable.TypePosition = ETypePosition.Static;
            ControlPanelTable.Style("margin", "auto");
            
            ControlPanelTableTr = new AraTableTr(ControlPanelTable);


            if (vSide == ESide.right || vSide == ESide.left)
                ControlPanelDiv.Width = 25;
            else if (vSide == ESide.bottom || vSide == ESide.top)
                ControlPanelDiv.Height = 32;

            Side = vSide;

            ControlPanelDiv.CssAddClass("ui-state-default");
        }

        [AraDevProperty(ESide.bottom)]
        public ESide Side
        {
            get { return _Side; }
            set
            {
                if (_Side != value)
                {
                    switch (_Side)
                    {
                        case ESide.bottom:
                            {
                                ControlPanelDiv.CssRemoveClass("ui-corner-bottom");
                            }
                            break;
                        case ESide.top:
                            {
                                ControlPanelDiv.CssRemoveClass("ui-corner-top");
                            }
                            break;
                        case ESide.left:
                            {
                                ControlPanelDiv.CssRemoveClass("ui-corner-left");
                            }
                            break;
                        case ESide.right:
                            {
                                ControlPanelDiv.CssRemoveClass("ui-corner-right");
                            }
                            break;
                    }



                    _Side = value;
                    switch (_Side)
                    {
                        case ESide.bottom:
                            {
                                Grid.Anchor.Top = 0;
                                Grid.Anchor.Left = 0;
                                Grid.Anchor.Right = 0;
                                Grid.Anchor.Bottom = (ControlPanelDiv.Height - 1).Value;

                                ControlPanelDiv.Anchor.Top = null;
                                ControlPanelDiv.Anchor.Left = 0;
                                ControlPanelDiv.Anchor.Right = 0;
                                ControlPanelDiv.Anchor.Bottom = 0;

                                ControlPanelDiv.CssAddClass("ui-corner-bottom");
                            }
                            break;
                        case ESide.top:
                            {
                                Grid.Anchor.Top = (ControlPanelDiv.Height - 1).Value;
                                Grid.Anchor.Left = 0;
                                Grid.Anchor.Right = 0;
                                Grid.Anchor.Bottom = 0;

                                ControlPanelDiv.Anchor.Top = 0;
                                ControlPanelDiv.Anchor.Left = 0;
                                ControlPanelDiv.Anchor.Right = 0;
                                ControlPanelDiv.Anchor.Bottom = null;

                                ControlPanelDiv.CssAddClass("ui-corner-top");
                            }
                            break;
                        case ESide.left:
                            {
                                Grid.Anchor.Top = 0;
                                Grid.Anchor.Left = (ControlPanelDiv.Width - 1).Value;
                                Grid.Anchor.Right = 0;
                                Grid.Anchor.Bottom = 0;

                                ControlPanelDiv.Anchor.Top = 0;
                                ControlPanelDiv.Anchor.Left = 0;
                                ControlPanelDiv.Anchor.Right = null;
                                ControlPanelDiv.Anchor.Bottom = 0;

                                ControlPanelDiv.CssAddClass("ui-corner-left");
                            }
                            break;
                        case ESide.right:
                            {
                                Grid.Anchor.Top = 0;
                                Grid.Anchor.Left = 0;
                                Grid.Anchor.Right = (ControlPanelDiv.Width - 1).Value;
                                Grid.Anchor.Bottom = 0;

                                ControlPanelDiv.Anchor.Top = 0;
                                ControlPanelDiv.Anchor.Left = null;
                                ControlPanelDiv.Anchor.Right = 0;
                                ControlPanelDiv.Anchor.Bottom = 0;

                                ControlPanelDiv.CssAddClass("ui-corner-right");
                            }
                            break;
                    }
                }
            }
        }

        public IAraContainerClient NewConteiner()
        {
            return new AraTableTd(ControlPanelTableTr);
        }

        private AraEvent<Action> _OnReloadControlPanel = new AraEvent<Action>();
        [AraDevEvent]
        public AraEvent<Action> OnReloadControlPanel
        {
            get { return _OnReloadControlPanel; }
            set { _OnReloadControlPanel = value; }
        }

        public void ReloadControlPanel()
        {
            try
            {
                if (OnReloadControlPanel.InvokeEvent != null)
                    OnReloadControlPanel.InvokeEvent();
            }
            catch (Exception err)
            {
                throw new Exception("Error in ReloadControlPanel.\n" + err.Message);
            }
        }

    }
}
