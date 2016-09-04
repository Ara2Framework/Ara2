// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
function ClassAraAnchor(vAraComponent) {
    this.AraComponent = vAraComponent;

    this.ConteinerFather=null;
    this.SetConteinerFather = function (vConteinerFather) {
        //if (this.ConteinerFather != null)
        //    throw "ConteinerFather already assigned";

        this.ConteinerFather = vConteinerFather;
        if (this.ConteinerFather != null)
            this.ConteinerFather.Anchor.AddChildren(this.AraComponent);
        else {
            var onResizeTolerancia = 0;
            if ($.browser.msie && $.browser.version < 9)
                onResizeTolerancia = 50;

            var TmpWidth001 = 0;
            var TmpHeight001 = 0;
            var TmpThis = this;
            function onResize() {
                var vDifW = TmpWidth001 - $(window).width();
                var vDifH = TmpHeight001 - $(window).height();

                if ((vDifW >= onResizeTolerancia || vDifW <= -onResizeTolerancia) || (vDifH >= onResizeTolerancia || vDifH <= -onResizeTolerancia)) {
                    TmpWidth001 = $(window).width();
                    TmpHeight001 = $(window).height();

                    TmpThis.AraComponent.SetWidth($(window).width());
                    TmpThis.AraComponent.SetHeight($(window).height());
                }
            }

            $(window).resize(function () {
                onResize();
            });
        }
    };

    this.Left == null;
    this.SetLeft = function (vValue) {
        this.Left = vValue;
        this.Render();
    };

    this.Top == null;
    this.SetTop = function (vValue) {
        this.Top = vValue;
        this.Render();
    };

    
    this.Right == null;
    this.SetRight = function (vValue) {
        this.Right = vValue;
        this.Render();
    };
    this.Bottom == null;
    this.SetBottom = function (vValue) {
        this.Bottom = vValue;
        this.Render();
    };

    this.GetConteinerFatherWidth = function () {
        if (this.ConteinerFather != null)
            return parseInt((this.ConteinerFather.Width==null?0:this.ConteinerFather.Width),10);
        else
            return $(window).width();
    };

    this.GetConteinerFatherHeight = function () {
        if (this.ConteinerFather != null)
            return parseInt((this.ConteinerFather.Height==null?0:this.ConteinerFather.Height),10);
        else
            return $(window).height();
    };

    this.RenderTimer= null;
    this.Render = function () {
        if (this.RenderTimer != null) {
            clearInterval(this.RenderTimer);
            this.RenderTimer = null;
        }
        
        var TmpThis = this;
        this.RenderTimer = setInterval(function () { TmpThis.RenderReal(); },10);
    };

    this.RenderReal = function () {
        if (this.RenderTimer != null) {
            clearInterval(this.RenderTimer);
            this.RenderTimer = null;
        }
        if (this.CenterH == false) {
            if (this.Left != null && this.Right != null) {

                var vWidth = this.GetConteinerFatherWidth() - this.Left - this.Right;

                this.AraComponent.SetLeft(this.Left + "px");
                this.AraComponent.SetWidth(vWidth + "px");
            }
            else if (this.Left != null) {
                this.AraComponent.SetLeft(this.Left + "px");
            }
            else if (this.Right != null) {
                var vLeft = this.GetConteinerFatherWidth() - parseInt(this.AraComponent.Width, 10) - this.Right;
                this.AraComponent.SetLeft(vLeft + "px");
            }
        }
        else {
            this.AraComponent.SetLeft(((this.GetConteinerFatherWidth() / 2) - (parseInt(this.AraComponent.Width,10) / 2)) + "px");
        }

        if (this.CenterV == false) {
            if (this.Top != null && this.Bottom != null) {
                var vHeight = this.GetConteinerFatherHeight() - this.Top - this.Bottom;

                this.AraComponent.SetTop(this.Top + "px");
                this.AraComponent.SetHeight(vHeight + "px");
            }
            else if (this.Top != null) {
                this.AraComponent.SetTop(this.Top + "px");
            }
            else if (this.Bottom != null) {
                var vTop = this.GetConteinerFatherHeight() - parseInt(this.AraComponent.Height, 10) - this.Bottom;
                this.AraComponent.SetTop(vTop + "px");
            }
        }
        else {
            this.AraComponent.SetTop(((this.GetConteinerFatherHeight() / 2) - (parseInt(this.AraComponent.Height,10) / 2)) + "px");
        }
    };

    this.RenderTimerChildren = null;
    this.RenderChildren = function () {
        if (this.RenderTimerChildren != null) {
            clearInterval(this.RenderTimerChildren);
            this.RenderTimerChildren = null;
        }

        var TmpThis = this;
        this.RenderTimerChildren = setInterval(function () { TmpThis.RenderChildrenReal(); }, 10);
    };

    this.RenderChildrenReal = function () {
        if (this.RenderTimerChildren != null) {
            clearInterval(this.RenderTimerChildren);
            this.RenderTimerChildren = null;
        }

        //  IE 7 
        //for (var vCObj in this.Children) {
        //    this.Children[vCObj].Anchor.RenderReal();
        //}

        // IE 7
        $(this.Children).each(function () {
            this.Anchor.RenderReal();
        });
    };

    this.Children = new Array();
    this.AddChildren = function (vChildren) {
        this.Children.push(vChildren);
    };

    this.Reflesh = function () {
        this.RenderReal();
    };

    this.CenterH = false;
    this.SetCenterH = function (vValue) {
        this.CenterH = vValue;

        if (this.CenterH == true) {
            this.Left == null;
            this.Top == null;
            this.Right == null;
            this.Bottom == null;
        }

        this.Render();
    };

    this.CenterV = false;
    this.SetCenterV = function (vValue) {
        this.CenterV = vValue;

        if (this.CenterV == true) {
            this.Left == null;
            this.Top == null;
            this.Right == null;
            this.Bottom == null;
        }

        this.Render();
    };

}