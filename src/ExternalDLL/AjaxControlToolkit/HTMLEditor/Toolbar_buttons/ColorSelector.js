Type.registerNamespace("AjaxControlToolkit.HTMLEditor.ToolbarButton");AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector=function(a){AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector.initializeBase(this,[a]);this._fixedColorButton=null};AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector.prototype={get_fixedColorButton:function(){return this._fixedColorButton},set_fixedColorButton:function(a){this._fixedColorButton=a},callMethod:function(){var a=this;if(!AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector.callBaseMethod(a,"callMethod"))return false;a.openPopup(Function.createDelegate(a,a._onopened));return true},_onopened:function(a){a.setColor=Function.createDelegate(this,this.setColor)},setColor:function(a){this.closePopup();this._fixedColorButton!=null&&this._fixedColorButton.set_defaultColor(a)}};AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector.registerClass("AjaxControlToolkit.HTMLEditor.ToolbarButton.ColorSelector",AjaxControlToolkit.HTMLEditor.ToolbarButton.Selector);