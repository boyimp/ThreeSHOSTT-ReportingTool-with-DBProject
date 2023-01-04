if(typeof window.RadControlsNamespace=="undefined"){
window.RadControlsNamespace={};
}
if(typeof (window.RadControlsNamespace.DomEventMixin)=="undefined"||typeof (window.RadControlsNamespace.DomEventMixin.Version)==null||window.RadControlsNamespace.DomEventMixin.Version<3){
RadControlsNamespace.DomEventMixin={Version:3,Initialize:function(_1){
_1.CreateEventHandler=this.CreateEventHandler;
_1.AttachDomEvent=this.AttachDomEvent;
_1.DetachDomEvent=this.DetachDomEvent;
_1.DisposeDomEventHandlers=this.DisposeDomEventHandlers;
_1._domEventHandlingEnabled=true;
_1.EnableDomEventHandling=this.EnableDomEventHandling;
_1.DisableDomEventHandling=this.DisableDomEventHandling;
_1.RemoveHandlerRegister=this.RemoveHandlerRegister;
_1.GetHandlerRegister=this.GetHandlerRegister;
_1.AddHandlerRegister=this.AddHandlerRegister;
_1.handlerRegisters=[];
},EnableDomEventHandling:function(){
this._domEventHandlingEnabled=true;
},DisableDomEventHandling:function(){
this._domEventHandlingEnabled=false;
},CreateEventHandler:function(_2,_3){
var _4=this;
return function(e){
if(!_4._domEventHandlingEnabled&&!_3){
return;
}
return _4[_2](e||window.event);
};
},AttachDomEvent:function(_6,_7,_8,_9){
var _a=this.CreateEventHandler(_8,_9);
var _b=this.GetHandlerRegister(_6,_7,_8);
if(_b!=null){
this.DetachDomEvent(_b.Element,_b.EventName,_8);
}
var _c={"Element":_6,"EventName":_7,"HandlerName":_8,"Handler":_a};
this.AddHandlerRegister(_c);
if(_6.addEventListener){
_6.addEventListener(_7,_a,false);
}else{
if(_6.attachEvent){
_6.attachEvent("on"+_7,_a);
}
}
},DetachDomEvent:function(_d,_e,_f){
var _10=null;
var _11="";
if(typeof _f=="string"){
_11=_f;
_10=this.GetHandlerRegister(_d,_e,_11);
if(_10==null){
return;
}
_f=_10.Handler;
}
if(!_d){
return;
}
if(_d.removeEventListener){
_d.removeEventListener(_e,_f,false);
}else{
if(_d.detachEvent){
_d.detachEvent("on"+_e,_f);
}
}
if(_10!=null&&_11!=""){
this.RemoveHandlerRegister(_10);
_10=null;
}
},DisposeDomEventHandlers:function(){
for(var i=0;i<this.handlerRegisters.length;i++){
var _13=this.handlerRegisters[i];
if(_13!=null){
this.DetachDomEvent(_13.Element,_13.EventName,_13.Handler);
}
}
this.handlerRegisters=[];
},RemoveHandlerRegister:function(_14){
try{
var _15=_14.index;
for(var i in _14){
_14[i]=null;
}
this.handlerRegisters[_15]=null;
}
catch(e){
}
},GetHandlerRegister:function(_17,_18,_19){
for(var i=0;i<this.handlerRegisters.length;i++){
var _1b=this.handlerRegisters[i];
if(_1b!=null&&_1b.Element==_17&&_1b.EventName==_18&&_1b.HandlerName==_19){
return this.handlerRegisters[i];
}
}
return null;
},AddHandlerRegister:function(_1c){
_1c.index=this.handlerRegisters.length;
this.handlerRegisters[this.handlerRegisters.length]=_1c;
}};
RadControlsNamespace.DomEvent={};
RadControlsNamespace.DomEvent.PreventDefault=function(e){
if(!e){
return true;
}
if(e.preventDefault){
e.preventDefault();
}
e.returnValue=false;
return false;
};
RadControlsNamespace.DomEvent.StopPropagation=function(e){
if(!e){
return;
}
if(e.stopPropagation){
e.stopPropagation();
}else{
e.cancelBubble=true;
}
};
RadControlsNamespace.DomEvent.GetTarget=function(e){
if(!e){
return null;
}
return e.target||e.srcElement;
};
RadControlsNamespace.DomEvent.GetRelatedTarget=function(e){
if(!e){
return null;
}
return e.relatedTarget||(e.type=="mouseout"?e.toElement:e.fromElement);
};
RadControlsNamespace.DomEvent.GetKeyCode=function(e){
if(!e){
return 0;
}
return e.which||e.keyCode;
};
}
var RadGridNamespace={};
RadGridNamespace.Prefix="grid_";
RadGridNamespace.InitializeClient=function(_22){
var _23=document.getElementById(_22+"AtlasCreation");
if(!_23){
return;
}
var _24=document.createElement("script");
if(navigator.userAgent.indexOf("Safari")!=-1){
_24.innerHTML=_23.innerHTML;
}else{
_24.text=_23.innerHTML;
}
if(!window.netscape){
document.body.appendChild(_24);
document.body.removeChild(_24);
}else{
document.body.insertBefore(_24,document.body.firstChild);
_24.parentNode.removeChild(_24);
}
_23.parentNode.removeChild(_23);
};
RadGridNamespace.AsyncRequest=function(_25,_26,_27,e){
var _29=window[_27];
if(_29!=null&&typeof (_29.AsyncRequest)=="function"){
_29.AsyncRequest(_25,_26,e);
}
};
RadGridNamespace.AsyncRequestWithOptions=function(_2a,_2b,e){
var _2d=window[_2b];
if(_2d!=null&&typeof (_2d.AsyncRequestWithOptions)=="function"){
_2d.AsyncRequestWithOptions(_2a,e);
}
};
RadGridNamespace.GetVisibleCols=function(_2e){
var _2f=0;
for(var i=0,l=_2e.length;i<l;i++){
if(_2e[i].style.display=="none"){
continue;
}
_2f++;
}
return _2f;
};
RadGridNamespace.HideShowCells=function(_32,_33,_34,_35){
var _36=RadGridNamespace.GetVisibleCols(_35);
for(var i=0,l=_32.rows.length;i<l;i++){
if(_32.rows[i].cells.length!=_36){
if(_32.rows[i].cells.length==1){
_32.rows[i].cells[0].colSpan=_36;
}else{
for(var j=0;j<_32.rows[i].cells.length;j++){
if(_32.rows[i].cells[j].colSpan>1&&j>=_33){
if(!_34){
_32.rows[i].cells[j].colSpan=_32.rows[i].cells[j].colSpan-1;
}else{
_32.rows[i].cells[j].colSpan=_32.rows[i].cells[j].colSpan+1;
}
break;
}
}
}
}
var _3a=_32.rows[i].cells[_33];
var _3b=(navigator.userAgent.toLowerCase().indexOf("safari")!=-1&&navigator.userAgent.indexOf("Mac")!=-1)?0:1;
if(!_34){
if(_3a!=null&&_3a.colSpan==_3b&&_3a.style.display!="none"){
_3a.style.display="none";
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&navigator.userAgent.toLowerCase().indexOf("6.0")!=-1){
RadGridNamespace.HideShowSelect(_3a,_34);
}
}
}else{
if(_3a!=null&&_3a.colSpan==_3b&&_3a.style.display=="none"){
_3a.style.display=(window.netscape)?"table-cell":"";
}
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&navigator.userAgent.toLowerCase().indexOf("6.0")!=-1){
RadGridNamespace.HideShowSelect(_3a,_34);
}
}
}
};
RadGridNamespace.HideShowSelect=function(_3c,_3d){
if(!_3c){
return;
}
var _3e=_3c.getElementsByTagName("select");
for(var i=0;i<_3e.length;i++){
_3e[i].style.display=(_3d)?"":"none";
}
};
RadGridNamespace.GetWidth=function(_40){
var _41;
if(window.getComputedStyle){
_41=window.getComputedStyle(_40,"").getPropertyValue("width");
}else{
if(_40.currentStyle){
_41=_40.currentStyle.width;
}else{
_41=_40.offsetWidth;
}
}
if(_41.toString().indexOf("%")!=-1){
_41=_40.offsetWidth;
}
if(_41.toString().indexOf("px")!=-1){
_41=parseInt(_41);
}
return _41;
};
RadGridNamespace.GetScrollBarWidth=function(){
try{
if(typeof (RadGridNamespace.scrollbarWidth)=="undefined"){
var _42,_43=0;
var _44=document.createElement("div");
_44.style.position="absolute";
_44.style.top="-1000px";
_44.style.left="-1000px";
_44.style.width="100px";
_44.style.overflow="auto";
var _45=document.createElement("div");
_45.style.width="1000px";
_44.appendChild(_45);
document.body.appendChild(_44);
_42=_44.offsetWidth;
_43=_44.clientWidth;
document.body.removeChild(document.body.lastChild);
RadGridNamespace.scrollbarWidth=_42-_43;
if(RadGridNamespace.scrollbarWidth<=0||_43==0){
RadGridNamespace.scrollbarWidth=16;
}
}
return RadGridNamespace.scrollbarWidth;
}
catch(error){
return false;
}
};
RadGridNamespace.GetScrollBarHeight=function(){
try{
if(typeof (RadGridNamespace.scrollbarHeight)=="undefined"){
var _46,_47=0;
var _48=document.createElement("div");
_48.style.position="absolute";
_48.style.top="-1000px";
_48.style.left="-1000px";
_48.style.width="100px";
_48.style.height="100px";
_48.style.overflow="auto";
var _49=document.createElement("div");
_49.style.width="1000px";
_49.style.height="1000px";
_48.appendChild(_49);
document.body.appendChild(_48);
_46=_48.offsetHeight;
_47=_48.clientHeight;
document.body.removeChild(document.body.lastChild);
RadGridNamespace.scrollbarHeight=_46-_47;
if(RadGridNamespace.scrollbarHeight<=0||_47==0){
RadGridNamespace.scrollbarHeight=16;
}
}
return RadGridNamespace.scrollbarHeight;
}
catch(error){
return false;
}
};
RadGridNamespace.GetTableColGroup=function(_4a){
try{
return _4a.getElementsByTagName("colgroup")[0];
}
catch(error){
return false;
}
};
RadGridNamespace.GetTableColGroupCols=function(_4b){
try{
var _4c=new Array();
var _4d=_4b.childNodes[0];
for(var i=0;i<_4b.childNodes.length;i++){
if((_4b.childNodes[i].tagName)&&(_4b.childNodes[i].tagName.toLowerCase()=="col")){
_4c[_4c.length]=_4b.childNodes[i];
}
}
return _4c;
}
catch(error){
return false;
}
};
RadGridNamespace.Confirm=function(_4f,e){
if(!confirm(_4f)){
e.cancelBubble=true;
e.returnValue=false;
return false;
}
};
RadGridNamespace.SynchronizeWithWindow=function(){
};
RadGridNamespace.IsRightToLeft=function(_51){
try{
while(_51){
if(_51.currentStyle&&_51.currentStyle.direction.toLowerCase()=="rtl"){
return true;
}else{
if(getComputedStyle&&getComputedStyle(_51,"").getPropertyValue("direction").toLowerCase()=="rtl"){
return true;
}else{
if(_51.dir.toLowerCase()=="rtl"){
return true;
}
}
}
_51=_51.parentNode;
}
return false;
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError,this.OnError);
}
};
RadGridNamespace.FireEvent=function(_52,_53,_54){
try{
var _55=true;
if(typeof (_52[_53])=="string"){
eval(_52[_53]);
}else{
if(typeof (_52[_53])=="function"){
if(_54){
switch(_54.length){
case 1:
_55=_52[_53](_54[0]);
break;
case 2:
_55=_52[_53](_54[0],_54[1]);
break;
}
}else{
_55=_52[_53]();
}
}
}
if(typeof (_55)!="boolean"){
return true;
}else{
return _55;
}
}
catch(error){
throw error;
}
};
RadGridNamespace.CheckParentNodesFor=function(_56,_57){
while(_56){
if(_56==_57){
return true;
}
_56=_56.parentNode;
}
return false;
};
RadGridNamespace.GetCurrentElement=function(e){
if(!e){
var e=window.event;
}
var _59;
if(e.srcElement){
_59=e.srcElement;
}else{
_59=e.target;
}
return _59;
};
RadGridNamespace.GetEventPosX=function(e){
var x=e.clientX;
var _5c=RadGridNamespace.GetCurrentElement(e);
while(_5c.parentNode){
if(typeof (_5c.parentNode.scrollLeft)=="number"){
x+=_5c.parentNode.scrollLeft;
}
_5c=_5c.parentNode;
}
if(document.compatMode=="BackCompat"||navigator.userAgent.indexOf("Safari")!=-1){
if(document.body.currentStyle&&document.body.currentStyle.margin&&document.body.currentStyle.margin.indexOf("px")!=-1&&!window.opera){
x=parseInt(x)-parseInt(document.body.currentStyle.marginLeft);
}
if(document.defaultView&&document.defaultView.getComputedStyle&&document.defaultView.getComputedStyle(document.body,"").marginLeft.indexOf("px")!=-1&&!window.opera){
x=parseInt(x)+parseInt(document.defaultView.getComputedStyle(document.body,"").marginLeft);
}
}
if(RadGridNamespace.IsRightToLeft(document.body)){
x=x-RadGridNamespace.GetScrollBarWidth();
}
return x;
};
RadGridNamespace.GetEventPosY=function(e){
var y=e.clientY;
var _5f=RadGridNamespace.GetCurrentElement(e);
while(_5f.parentNode){
if(typeof (_5f.parentNode.scrollTop)=="number"){
y+=_5f.parentNode.scrollTop;
}
_5f=_5f.parentNode;
}
if(document.compatMode=="BackCompat"||navigator.userAgent.indexOf("Safari")!=-1){
if(document.body.currentStyle&&document.body.currentStyle.margin&&document.body.currentStyle.margin.indexOf("px")!=-1&&!window.opera){
y=parseInt(y)-parseInt(document.body.currentStyle.marginTop);
}
if(document.defaultView&&document.defaultView.getComputedStyle&&document.defaultView.getComputedStyle(document.body,"").marginTop.indexOf("px")!=-1&&!window.opera){
y=parseInt(y)+parseInt(document.defaultView.getComputedStyle(document.body,"").marginTop);
}
}
return y;
};
RadGridNamespace.IsChildOf=function(_60,_61){
while(_60.parentNode){
if(_60.parentNode==_61){
return true;
}
_60=_60.parentNode;
}
return false;
};
RadGridNamespace.GetFirstParentByTagName=function(_62,_63){
while(_62.parentNode){
if(_62.tagName.toLowerCase()==_63.toLowerCase()){
return _62;
}
_62=_62.parentNode;
}
return null;
};
RadGridNamespace.FindScrollPosX=function(_64){
var x=0;
while(_64.parentNode){
if(typeof (_64.parentNode.scrollLeft)=="number"){
x+=_64.parentNode.scrollLeft;
}
_64=_64.parentNode;
}
if(document.body.currentStyle&&document.body.currentStyle.margin&&document.body.currentStyle.margin.indexOf("px")!=-1&&!window.opera){
x=parseInt(x)-parseInt(document.body.currentStyle.marginLeft);
}
return x;
};
RadGridNamespace.FindScrollPosY=function(_66){
var y=0;
while(_66.parentNode){
if(typeof (_66.parentNode.scrollTop)=="number"){
y+=_66.parentNode.scrollTop;
}
_66=_66.parentNode;
}
if(document.body.currentStyle&&document.body.currentStyle.margin&&document.body.currentStyle.margin.indexOf("px")!=-1&&!window.opera){
y=parseInt(y)-parseInt(document.body.currentStyle.marginTop);
}
return y;
};
RadGridNamespace.FindPosX=function(_68){
try{
var x=0;
var _6a=0;
if(_68.offsetParent){
while(_68.offsetParent){
x+=_68.offsetLeft;
if(_68.currentStyle&&_68.currentStyle.borderLeftWidth&&_68.currentStyle.borderLeftWidth.indexOf("px")!=-1&&!window.opera){
_6a+=parseInt(_68.currentStyle.borderLeftWidth);
}
_68=_68.offsetParent;
}
}else{
if(_68.x){
x+=_68.x;
}
}
if(document.compatMode=="BackCompat"||navigator.userAgent.indexOf("Safari")!=-1){
if(document.body.currentStyle&&document.body.currentStyle.margin&&document.body.currentStyle.margin.indexOf("px")!=-1&&!window.opera){
x=parseInt(x)-parseInt(document.body.currentStyle.marginLeft);
}
if(document.defaultView&&document.defaultView.getComputedStyle&&document.defaultView.getComputedStyle(document.body,"").marginLeft.indexOf("px")!=-1&&!window.opera){
x=parseInt(x)+parseInt(document.defaultView.getComputedStyle(document.body,"").marginLeft);
}
}
return x+_6a;
}
catch(error){
return x;
}
};
RadGridNamespace.FindPosY=function(_6b){
var y=0;
var _6d=0;
if(_6b.offsetParent){
while(_6b.offsetParent){
y+=_6b.offsetTop;
if(_6b.currentStyle&&_6b.currentStyle.borderTopWidth&&_6b.currentStyle.borderTopWidth.indexOf("px")!=-1&&!window.opera){
_6d+=parseInt(_6b.currentStyle.borderTopWidth);
}
_6b=_6b.offsetParent;
}
}else{
if(_6b.y){
y+=_6b.y;
}
}
if(document.compatMode=="BackCompat"||navigator.userAgent.indexOf("Safari")!=-1){
if(document.body.currentStyle&&document.body.currentStyle&&document.body.currentStyle.margin.indexOf("px")!=-1&&!window.opera){
y=parseInt(y)-parseInt(document.body.currentStyle.marginTop);
}
if(document.defaultView&&document.defaultView.getComputedStyle&&document.defaultView.getComputedStyle(document.body,"").marginTop.indexOf("px")!=-1&&!window.opera){
y=parseInt(y)+parseInt(document.defaultView.getComputedStyle(document.body,"").marginTop);
}
}
return y+_6d;
};
RadGridNamespace.GetNodeNextSiblingByTagName=function(_6e,_6f){
while((_6e!=null)&&(_6e.tagName!=_6f)){
_6e=_6e.nextSibling;
}
return _6e;
};
RadGridNamespace.GetNodeNextSibling=function(_70){
while(_70!=null){
if(_70.nextSibling){
_70=_70.nextSibling;
}else{
_70=null;
}
if(_70){
if(_70.nodeType==1){
break;
}
}
}
return _70;
};
RadGridNamespace.DeleteSubString=function(_71,_72,_73){
return _71=_71.substring(0,_72)+_71.substring(_73+1,_71.length);
};
RadGridNamespace.ClearDocumentEvents=function(){
if(document.onmousedown!=this.mouseDownHandler){
this.documentOnMouseDown=document.onmousedown;
}
if(document.onselectstart!=this.selectStartHandler){
this.documentOnSelectStart=document.onselectstart;
}
if(document.ondragstart!=this.dragStartHandler){
this.documentOnDragStart=document.ondragstart;
}
this.mouseDownHandler=function(e){
return false;
};
this.selectStartHandler=function(){
return false;
};
this.dragStartHandler=function(){
return false;
};
document.onmousedown=this.mouseDownHandler;
document.onselectstart=this.selectStartHandler;
document.ondragstart=this.dragStartHandler;
};
RadGridNamespace.RestoreDocumentEvents=function(){
if((typeof (this.documentOnMouseDown)=="function")&&(document.onmousedown!=this.mouseDownHandler)){
document.onmousedown=this.documentOnMouseDown;
}else{
document.onmousedown="";
}
if((typeof (this.documentOnSelectStart)=="function")&&(document.onselectstart!=this.selectStartHandler)){
document.onselectstart=this.documentOnSelectStart;
}else{
document.onselectstart="";
}
if((typeof (this.documentOnDragStart)=="function")&&(document.ondragstart!=this.dragStartHandler)){
document.ondragstart=this.documentOnDragStart;
}else{
document.ondragstart="";
}
};
RadGridNamespace.AddStyleSheet=function(_75){
if(RadGridNamespace.StyleSheets==null){
RadGridNamespace.StyleSheets={};
}
var _76=RadGridNamespace.StyleSheets[_75];
if(_76!=null){
return null;
}
var css=null;
var _78=null;
var _79=document.getElementsByTagName("head")[0];
if(window.netscape||navigator.userAgent.indexOf("Safari")!=-1){
css=document.createElement("style");
css.media="all";
css.type="text/css";
_79.appendChild(css);
}else{
try{
css=document.createStyleSheet();
}
catch(e){
return false;
}
}
var _7a=document.styleSheets[document.styleSheets.length-1];
RadGridNamespace.StyleSheets[_75]=_7a;
return _7a;
};
RadGridNamespace.AddRule=function(ss,_7c,_7d){
try{
if(!ss){
return false;
}
if(ss.insertRule&&navigator.userAgent.indexOf("Safari")==-1){
var _7e=ss.insertRule(_7c+" {"+_7d+"}",ss.cssRules.length);
return ss.cssRules[ss.cssRules.length-1];
}
if(navigator.userAgent.indexOf("Safari")!=-1){
ss.addRule(_7c,_7d);
return ss.cssRules[ss.cssRules.length-1];
}
if(ss.addRule){
ss.addRule(_7c,_7d);
return true;
}
return false;
}
catch(e){
return false;
}
};
RadGridNamespace.addClassName=function(_7f,_80){
var s=_7f.className;
var p=s.split(" ");
if(p.length==1&&p[0]==""){
p=[];
}
var l=p.length;
for(var i=0;i<l;i++){
if(p[i]==_80){
return;
}
}
p[p.length]=_80;
_7f.className=p.join(" ");
};
RadGridNamespace.removeClassName=function(_85,_86){
if(_85.className.replace(/^\s*|\s*$/g,"")==_86){
_85.className="";
return;
}
var _87=_85.className.split(" ");
var _88=[];
for(var i=0,l=_87.length;i<l;i++){
if(_87[i]==""){
continue;
}
if(_86.indexOf(_87[i])==-1){
_88[_88.length]=_87[i];
}
}
_85.className=_88.join(" ");
return;
_85.className=(_85.className.toString()==_86)?"":_85.className.replace(_86,"").replace(/\s*$/g,"");
return;
var p=s.split(" ");
var np=[];
var l=p.length;
var j=0;
for(var i=0;i<l;i++){
if(p[i]!=_86){
np[j++]=p[i];
}
}
_85.className=np.join(" ");
};
RadGridNamespace.CheckIsParentDisplay=function(_8e){
try{
while(_8e){
if(_8e.style){
if(_8e.currentStyle){
if(_8e.currentStyle.display=="none"){
return false;
}
}else{
if(_8e.style.display=="none"){
return false;
}
}
}
_8e=_8e.parentNode;
}
if(window.top){
if(window.top.location!=window.location){
return false;
}
}
return true;
}
catch(e){
return false;
}
};
RadGridNamespace.EncodeURI=function(_8f){
if(encodeURI){
return encodeURI(_8f);
}else{
return escape(_8f);
}
};
if(typeof (window.RadControlsNamespace)=="undefined"){
window.RadControlsNamespace=new Object();
}
RadControlsNamespace.AppendStyleSheet=function(_90,_91,_92){
if(!_92){
return;
}
if(!_90){
document.write("<"+"link"+" rel='stylesheet' type='text/css' href='"+_92+"' />");
}else{
var _93=document.createElement("link");
_93.rel="stylesheet";
_93.type="text/css";
_93.href=_92;
var _94=document.getElementById(_91+"StyleSheetHolder");
if(_94!=null){
document.getElementById(_91+"StyleSheetHolder").appendChild(_93);
}
}
};
RadGridNamespace.RadGrid=function(_95){
var _96=window[_95.ClientID];
if(_96!=null&&typeof (_96.Dispose)=="function"){
window.setTimeout(function(){
_96.Dispose();
},100);
}
RadControlsNamespace.DomEventMixin.Initialize(this);
this.AttachDomEvent(window,"unload","OnWindowUnload");
window[_95.ClientID]=this;
window["grid_"+_95.ClientID]=this;
if(RadGridNamespace.DocumentCanBeModified()){
this._constructor(_95);
}else{
this.objectData=_95;
this.AttachDomEvent(window,"load","OnWindowLoad");
}
};
RadGridNamespace.DocumentCanBeModified=function(){
return (RadGridNamespace.DocumentHasBeenFullyLoaded==true)||(document.readyState=="complete")||window.opera||window.netscape;
};
RadGridNamespace.RadGrid.prototype.OnWindowUnload=function(e){
this.Dispose();
};
RadGridNamespace.RadGrid.prototype.OnWindowLoad=function(e){
RadGridNamespace.DocumentHasBeenFullyLoaded=true;
this._constructor(this.objectData);
this.objectData=null;
};
RadGridNamespace.RadGrid.prototype._constructor=function(_99){
this.Type="RadGrid";
if(_99.ClientSettings){
this.InitializeEvents(_99.ClientSettings.ClientEvents);
}
RadGridNamespace.FireEvent(this,"OnGridCreating");
for(var _9a in _99){
this[_9a]=_99[_9a];
}
this.Initialize();
RadGridNamespace.FireEvent(this,"OnMasterTableViewCreating");
this.GridStyleSheet=RadGridNamespace.AddStyleSheet(this.ClientID);
if(this.ClientSettings&&this.ClientSettings.Scrolling.AllowScroll&&this.ClientSettings.Scrolling.UseStaticHeaders){
var ID=_99.MasterTableView.ClientID;
_99.MasterTableView.ClientID=ID+"_Header";
this.MasterTableViewHeader=new RadGridNamespace.RadGridTable(_99.MasterTableView);
this.MasterTableViewHeader._constructor(this);
if(document.getElementById(ID+"_Footer")){
_99.MasterTableView.ClientID=ID+"_Footer";
this.MasterTableViewFooter=new RadGridNamespace.RadGridTable(_99.MasterTableView);
this.MasterTableViewFooter._constructor(this);
}
_99.MasterTableView.ClientID=ID;
}
this.MasterTableView._constructor(this);
RadGridNamespace.FireEvent(this,"OnMasterTableViewCreated");
this.DetailTablesCollection=new Array();
this.LoadDetailTablesCollection(this.MasterTableView,1);
this.AttachDomEvents();
this.InitializeFeatures(_99);
RadGridNamespace.FireEvent(this,"OnGridCreated");
if(typeof (window.event)=="undefined"){
window.event=null;
}
};
RadGridNamespace.RadGrid.prototype.Dispose=function(){
if(this.Disposed){
return;
}
this.Disposed=true;
try{
RadGridNamespace.FireEvent(this,"OnGridDestroying");
this.DisposeDomEventHandlers();
this.DisposeEvents();
this.GridStyleSheet=null;
this.DisposeFeatures();
this.DisposeDetailTablesCollection(this.MasterTableView,1);
if(this.MasterTableViewHeader!=null){
this.MasterTableViewHeader.Dispose();
}
if(this.MasterTableViewFooter!=null){
this.MasterTableViewFooter.Dispose();
}
if(this.MasterTableView!=null){
this.MasterTableView.Dispose();
}
this.DisposeProperties();
}
catch(error){
}
};
RadGridNamespace.RadGrid.ClientEventNames={OnGridCreating:true,OnGridCreated:true,OnGridDestroying:true,OnMasterTableViewCreating:true,OnMasterTableViewCreated:true,OnTableCreating:true,OnTableCreated:true,OnTableDestroying:true,OnScroll:true,OnKeyPress:true,OnRequestStart:true,OnRequestEnd:true,OnRequestError:true,OnError:true,OnRowDeleting:true,OnRowDeleted:true};
RadGridNamespace.RadGrid.prototype.IsClientEventName=function(_9c){
return RadGridNamespace.RadGrid.ClientEventNames[_9c]==true;
};
RadGridNamespace.RadGrid.prototype.InitializeEvents=function(_9d){
for(var _9e in _9d){
if(typeof (_9d[_9e])!="string"){
continue;
}
if(this.IsClientEventName(_9e)){
if(_9d[_9e]!=""){
var _9f=_9d[_9e];
if(_9f.indexOf("(")!=-1){
this[_9e]=_9f;
}else{
this[_9e]=eval(_9f);
}
}else{
this[_9e]=null;
}
}
}
};
RadGridNamespace.RadGrid.prototype.DisposeEvents=function(){
for(var _a0 in RadGridNamespace.RadGrid.ClientEventNames){
this[_a0]=null;
}
};
RadGridNamespace.RadGrid.prototype.GetDetailTable=function(_a1,_a2){
if(_a1.HierarchyIndex==_a2){
return _a1;
}
if(_a1.DetailTables){
for(var i=0;i<_a1.DetailTables.length;i++){
var res=this.GetDetailTable(_a1.DetailTables[i],_a2);
if(res){
return res;
}
}
}
};
RadGridNamespace.RadGrid.prototype.LoadDetailTablesCollection=function(_a5,_a6){
try{
if(_a5.Controls[0]!=null&&_a5.Controls[0].Rows!=null){
for(var i=0;i<_a5.Controls[0].Rows.length;i++){
var _a8=_a5.Controls[0].Rows[i].ItemType;
if(_a8=="NestedView"){
var _a9=_a5.Controls[0].Rows[i].NestedTableViews;
for(var j=0;j<_a9.length;j++){
var _ab=_a9[j];
if(_ab.Visible){
var _ac=this.GetDetailTable(this.MasterTableView,_ab.HierarchyIndex);
RadGridNamespace.FireEvent(this,"OnTableCreating",[_ac]);
_ab._constructor(this);
this.DetailTablesCollection[this.DetailTablesCollection.length]=_ab;
if(_ab.AllowFilteringByColumn){
this.InitializeFilterMenu(_ab);
}
RadGridNamespace.FireEvent(this,"OnTableCreated",[_ab]);
}
this.LoadDetailTablesCollection(_ab,_a6+1);
}
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.DisposeDetailTablesCollection=function(_ad,_ae){
if(_ad.Controls[0]!=null&&_ad.Controls[0].Rows!=null){
for(var i=0;i<_ad.Controls[0].Rows.length;i++){
var _b0=_ad.Controls[0].Rows[i].ItemType;
if(_b0=="NestedView"){
var _b1=_ad.Controls[0].Rows[i].NestedTableViews;
for(var j=0;j<_b1.length;j++){
var _b3=_b1[j];
_b3.Dispose();
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.AddRtlClass=function(){
if(RadGridNamespace.IsRightToLeft(this.Control)){
RadGridNamespace.addClassName(this.Control,"RadGridRTL_"+this.Skin);
}
};
RadGridNamespace.RadGrid.prototype.Initialize=function(){
this.Control=document.getElementById(this.ClientID);
if(this.Control==null){
return;
}
this.Control.tabIndex=0;
this.AddRtlClass();
this.GridDataDiv=document.getElementById(this.ClientID+"_GridData");
if(this.GroupPanel){
this.GroupPanelControl=document.getElementById(this.GroupPanel.ClientID+"_GroupPanel");
}
this.GridHeaderDiv=document.getElementById(this.ClientID+"_GridHeader");
this.GridFooterDiv=document.getElementById(this.ClientID+"_GridFooter");
this.PostDataValue=document.getElementById(this.ClientID+"PostDataValue");
this.LoadingTemplate=document.getElementById(this.ClientID+"_LoadingTemplate");
this.PagerControl=document.getElementById(this.MasterTableView.ClientID+"_Pager");
this.TopPagerControl=document.getElementById(this.MasterTableView.ClientID+"_TopPager");
if(this.LoadingTemplate){
this.LoadingTemplate.style.display="none";
if(this.GridDataDiv){
this.GridDataDiv.appendChild(this.LoadingTemplate);
}
}
};
RadGridNamespace.RadGrid.prototype.DisposeProperties=function(){
this.Control=null;
this.GridDataDiv=null;
this.GroupPanelControl=null;
this.GridHeaderDiv=null;
this.GridFooterDiv=null;
this.PostDataValue=null;
this.LoadingTemplate=null;
this.PagerControl=null;
};
RadGridNamespace.RadGrid.prototype.InitializeFeatures=function(_b4){
if(!this.MasterTableView.Control){
return;
}
if(this.GroupPanelControl!=null){
this.GroupPanelObject=new RadGridNamespace.RadGridGroupPanel(this.GroupPanelControl,this);
}
if(this.ClientSettings&&this.ClientSettings.Scrolling.AllowScroll){
this.InitializeDimensions();
this.InitializeScroll();
}
if(this.AllowFilteringByColumn||this.MasterTableView.AllowFilteringByColumn){
var _b5=(this.MasterTableViewHeader)?this.MasterTableViewHeader:this.MasterTableView;
this.InitializeFilterMenu(_b5);
}
if(this.ClientSettings&&this.ClientSettings.AllowKeyboardNavigation&&this.MasterTableView.Rows){
if(!this.MasterTableView.RenderActiveItemStyleClass||this.MasterTableView.RenderActiveItemStyleClass==""){
if(this.MasterTableView.RenderActiveItemStyle&&this.MasterTableView.RenderActiveItemStyle!=""){
RadGridNamespace.AddRule(this.GridStyleSheet,".ActiveItemStyle"+this.MasterTableView.ClientID+"1 td",this.MasterTableView.RenderActiveItemStyle);
}else{
RadGridNamespace.AddRule(this.GridStyleSheet,".ActiveItemStyle"+this.MasterTableView.ClientID+"2 td","background-color:#FFA07A;");
}
}
if(this.ActiveRow==null){
this.ActiveRow=this.MasterTableView.Rows[0];
}
this.SetActiveRow(this.ActiveRow);
}
if(this.ClientSettings&&this.ClientSettings.Slider!=null&&this.ClientSettings.Slider!=""){
eval(this.ClientSettings.Slider);
}
if(window[this.ClientID+"_Slider"]){
this.Slider=new RadGridNamespace.Slider(window[this.ClientID+"_Slider"]);
}
};
RadGridNamespace.RadGrid.prototype.DisposeFeatures=function(){
if(this.Slider!=null){
this.Slider.Dispose();
this.Slider=null;
}
if(this.GroupPanelControl!=null){
this.GroupPanelObject.Dispose();
this.GroupPanelControl=null;
}
if(this.AllowFilteringByColumn||this.MasterTableView.AllowFilteringByColumn){
var _b6=(this.MasterTableViewHeader)?this.MasterTableViewHeader:this.MasterTableView;
this.DisposeFilterMenu(_b6);
}
this.Control=null;
};
RadGridNamespace.RadGrid.prototype.AsyncRequest=function(_b7,_b8,e){
var _ba;
if(this.StatusBarSettings!=null&&this.StatusBarSettings.StatusLabelID!=null&&this.StatusBarSettings.StatusLabelID!=""){
var _bb=document.getElementById(this.StatusBarSettings.StatusLabelID);
if(_bb!=null){
_ba=_bb.innerHTML;
_bb.innerHTML=this.StatusBarSettings.LoadingText;
}
}
var _bc=this.ClientID;
this.OnRequestEndInternal=function(){
RadGridNamespace.FireEvent(window[_bc],"OnRequestEnd");
if(_bb){
_bb.innerHTML=_ba;
}
};
RadAjaxNamespace.AsyncRequest(_b7,_b8,_bc,e);
};
RadGridNamespace.RadGrid.prototype.AjaxRequest=function(_bd,_be){
this.AsyncRequest(_bd,_be);
};
RadGridNamespace.RadGrid.prototype.ClearSelectedRows=function(){
for(var i=0;i<this.DetailTablesCollection.length;i++){
var _c0=this.DetailTablesCollection[i];
_c0.ClearSelectedRows();
}
this.MasterTableView.ClearSelectedRows();
};
RadGridNamespace.RadGrid.prototype.AsyncRequestWithOptions=function(_c1,e){
RadAjaxNamespace.AsyncRequestWithOptions(_c1,this.ClientID,e);
};
RadGridNamespace.RadGrid.prototype.DeleteRow=function(_c3,_c4,e){
var _c6=(e.srcElement)?e.srcElement:e.target;
if(!_c6){
return;
}
var row=_c6.parentNode.parentNode;
var _c8=row.parentNode.parentNode;
var _c9=row.rowIndex;
var _ca=row.cells.length;
var _cb=this.GetTableObjectByID(_c3);
var _cc=this.GetRowObjectByRealRow(_cb,row);
var _cd={Row:_cc};
if(!RadGridNamespace.FireEvent(this,"OnRowDeleting",[_cb,_cd])){
return;
}
_c8.deleteRow(row.rowIndex);
for(var i=_c9;i<_c8.rows.length;i++){
if(_c8.rows[i].cells.length!=_ca&&_c8.rows[i].style.display!="none"){
_c8.deleteRow(i);
i--;
}else{
break;
}
}
if(_c8.tBodies[0].rows.length==1&&_c8.tBodies[0].rows[0].style.display=="none"){
_c8.tBodies[0].rows[0].style.display="";
}
if(_cb.IsInSelectedRows(_cc)){
_cb.RemoveFromSelectedRows(_cc);
}
this.PostDataValue.value+="DeletedRows,"+_c3+","+_c4+";";
if(_cc){
_cb.RemoveRow(_cc);
}
RadGridNamespace.FireEvent(this,"OnRowDeleted",[_cb,_cd]);
};
RadGridNamespace.RadGrid.prototype.SelectRow=function(_cf,_d0,e){
var _d2=(e.srcElement)?e.srcElement:e.target;
if(!_d2){
return;
}
var row=RadGridNamespace.GetFirstParentByTagName(_d2,"tr");
var _d4=RadGridNamespace.GetFirstParentByTagName(row,"table");
var _d5=row.rowIndex;
var _d6;
if(_cf==this.MasterTableView.UID){
_d6=this.MasterTableView;
}else{
for(var i=0;i<this.DetailTablesCollection.length;i++){
if(this.DetailTablesCollection[i].ClientID==_d4.id){
_d6=this.DetailTablesCollection[i];
break;
}
}
}
if(_d6!=null){
if(this.AllowMultiRowSelection){
_d6.SelectRow(row,false);
}else{
_d6.SelectRow(row,true);
}
}
if(this.ClientSettings.EnablePostBackOnRowClick){
var _d8=this.ClientSettings.PostBackFunction;
_d8=_d8.replace("{0}",this.UniqueID).replace("{1}","RowClick;"+_d0);
var _d9=document.getElementById(this.FormID);
if(_d9!=null&&_d9["__EVENTTARGET"]!=null&&_d9["__EVENTTARGET"].value==this.UniqueID){
_d9["__EVENTTARGET"].value="";
}
if(_d9!=null&&_d9["__EVENTTARGET"]!=null&&_d9["__EVENTTARGET"].value==""){
eval(_d8);
}
}
};
RadGridNamespace.RadGrid.prototype.SelectAllRows=function(_da,_db,e){
var _dd=(e.srcElement)?e.srcElement:e.target;
if(!_dd){
return;
}
var row=_dd.parentNode.parentNode;
var _df=row.parentNode.parentNode;
var _e0=row.rowIndex;
var _e1;
if(_da==this.MasterTableView.UID){
_e1=this.MasterTableView;
}else{
for(var i=0;i<this.DetailTablesCollection.length;i++){
if(this.DetailTablesCollection[i].UID==_da){
_e1=this.DetailTablesCollection[i];
break;
}
}
}
if(_e1!=null){
if(this.AllowMultiRowSelection){
if(_e1==this.MasterTableViewHeader){
_e1=this.MasterTableView;
}
_e1.ClearSelectedRows();
if(_dd.checked){
for(var i=0;i<_e1.Control.tBodies[0].rows.length;i++){
var row=_e1.Control.tBodies[0].rows[i];
_e1.SelectRow(row,false);
}
}else{
for(var i=0;i<_e1.Control.tBodies[0].rows.length;i++){
var row=_e1.Control.tBodies[0].rows[i];
_e1.DeselectRow(row);
}
this.UpdateClientRowSelection();
}
}
if(this.ClientSettings.EnablePostBackOnRowClick){
var _e3=this.ClientSettings.PostBackFunction;
_e3=_e3.replace("{0}",this.UniqueID).replace("{1}","RowClick;"+_db);
var _e4=document.getElementById(this.FormID);
if(_e4!=null&&_e4["__EVENTTARGET"]!=null&&_e4["__EVENTTARGET"].value==this.UniqueID){
_e4["__EVENTTARGET"].value="";
}
if(_e4!=null&&_e4["__EVENTTARGET"]!=null&&_e4["__EVENTTARGET"].value==""){
eval(_e3);
}
}
}
};
RadGridNamespace.RadGrid.prototype.UpdateClientRowSelection=function(){
var _e5=this.MasterTableView.GetSelectedRowsIndexes();
this.SavePostData("SelectedRows",this.MasterTableView.ClientID,_e5);
for(var i=0;i<this.DetailTablesCollection.length;i++){
_e5=this.DetailTablesCollection[i].GetSelectedRowsIndexes();
this.SavePostData("SelectedRows",this.DetailTablesCollection[i].ClientID,_e5);
}
};
RadGridNamespace.RadGrid.prototype.HandleActiveRow=function(e){
if((this.AllowRowResize)||(this.AllowRowSelect)){
var _e8=this.GetCellFromPoint(e);
if((_e8!=null)&&(_e8.parentNode.id!="")&&(_e8.parentNode.id!=-1)&&(_e8.cellIndex==0)){
var _e9=_e8.parentNode.parentNode.parentNode;
this.SetActiveRow(_e9,_e8.parentNode.rowIndex);
}
}
};
RadGridNamespace.RadGrid.prototype.SetActiveRow=function(_ea){
if(_ea==null){
return;
}
if(_ea.Owner.RenderActiveItemStyle){
RadGridNamespace.removeClassName(this.ActiveRow.Control,"ActiveItemStyle"+_ea.Owner.ClientID+"1");
}else{
RadGridNamespace.removeClassName(this.ActiveRow.Control,"ActiveItemStyle"+_ea.Owner.ClientID+"2");
}
RadGridNamespace.removeClassName(this.ActiveRow.Control,_ea.Owner.RenderActiveItemStyleClass);
if(this.ActiveRow.Control.style.cssText==_ea.Owner.RenderActiveItemStyle){
this.ActiveRow.Control.style.cssText="";
}
this.ActiveRow=_ea;
if(!this.ActiveRow.Owner.RenderActiveItemStyleClass||this.ActiveRow.Owner.RenderActiveItemStyleClass==""){
if(this.ActiveRow.Owner.RenderActiveItemStyle&&this.ActiveRow.Owner.RenderActiveItemStyle!=""){
RadGridNamespace.addClassName(this.ActiveRow.Control,"ActiveItemStyle"+this.ActiveRow.Owner.ClientID+"1");
}else{
RadGridNamespace.addClassName(this.ActiveRow.Control,"ActiveItemStyle"+this.ActiveRow.Owner.ClientID+"2");
}
}else{
RadGridNamespace.addClassName(this.ActiveRow.Control,this.ActiveRow.Owner.RenderActiveItemStyleClass);
}
this.SavePostData("ActiveRow",this.ActiveRow.Owner.ClientID,this.ActiveRow.RealIndex);
};
RadGridNamespace.RadGrid.prototype.GetNextRow=function(_eb,_ec){
if(_eb!=null){
if(_eb.tBodies[0].rows[_ec]!=null){
while(_eb.tBodies[0].rows[_ec]!=null){
_ec++;
if(_ec<=(_eb.tBodies[0].rows.length-1)){
return _eb.tBodies[0].rows[_ec];
}else{
return null;
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.GetPreviousRow=function(_ed,_ee){
if(_ed!=null){
if(_ed.tBodies[0].rows[_ee]!=null){
while(_ed.tBodies[0].rows[_ee]!=null){
_ee--;
if(_ee>=0){
return _ed.tBodies[0].rows[_ee];
}else{
return null;
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.GetNextHierarchicalRow=function(_ef,_f0){
if(_ef!=null){
if(_ef.tBodies[0].rows[_f0]!=null){
_f0++;
var row=_ef.tBodies[0].rows[_f0];
if(_ef.tBodies[0].rows[_f0]!=null){
if((row.cells[1]!=null)&&(row.cells[2]!=null)){
if((row.cells[1].getElementsByTagName("table").length>0)||(row.cells[2].getElementsByTagName("table").length>0)){
var _f2=this.GetNextRow(row.cells[2].firstChild,0);
return _f2;
}else{
return null;
}
}
}
}
}
};
RadGridNamespace.RadGrid.prototype.GetPreviousHierarchicalRow=function(_f3,_f4){
if(_f3!=null){
if(_f3.parentNode!=null){
if(_f3.parentNode.tagName.toLowerCase()=="td"){
var _f5=_f3.parentNode.parentNode.parentNode.parentNode;
var _f6=_f3.parentNode.parentNode.rowIndex;
return this.GetPreviousRow(_f5,_f6);
}else{
return null;
}
}else{
return this.GetPreviousRow(_f3,_f4);
}
}
};
RadGridNamespace.RadGrid.prototype.HandleCellEdit=function(e){
var _f8=RadGridNamespace.GetCurrentElement(e);
var _f9=RadGridNamespace.GetFirstParentByTagName(_f8,"td");
if(_f9!=null){
_f8=_f9;
var _fa=_f8.parentNode.parentNode.parentNode;
var _fb=this.GetTableObjectByID(_fa.id);
if((_fb!=null)&&(_fb.Columns.length>0)&&(_fb.Columns[_f8.cellIndex]!=null)){
if(_fb.Columns[_f8.cellIndex].ColumnType!="GridBoundColumn"){
return;
}
this.EditedCell=_fb.Control.rows[_f8.parentNode.rowIndex].cells[_f8.cellIndex];
this.CellEditor=new RadGridNamespace.RadGridCellEditor(this.EditedCell,_fb.Columns[_f8.cellIndex],this);
}
}
};
RadGridNamespace.RadGridCellEditor=function(_fc,_fd,_fe){
if(_fe.CellEditor){
return;
}
this.Control=document.createElement("input");
this.Control.style.border="1px groove";
this.Control.style.width="100%";
this.Control.value=_fc.innerHTML;
this.OldValue=this.Control.value;
_fc.innerHTML="";
var _ff=this;
this.Control.onblur=function(e){
if(!e){
var e=window.event;
}
_fc.removeChild(this);
_fc.innerHTML=this.value;
if(this.value!=_ff.OldValue){
alert(1);
}
_fe.CellEditor=null;
};
_fc.appendChild(this.Control);
if(this.Control.focus){
this.Control.focus();
}
};
if(!("console" in window)||!("firebug" in console)){
var names=["log","debug","info","warn","error","assert","dir","dirxml","group","groupEnd","time","timeEnd","count","trace","profile","profileEnd"];
window.console={};
for(var i=0;i<names.length;++i){
window.console[names[i]]=function(){
};
}
}
RadGridNamespace.Error=function(_101,_102,_103){
if((!_101)||(!_102)||(!_103)){
return false;
}
this.Message=_101.message;
if(_103!=null){
if("string"==typeof (_103)){
try{
eval(_103);
}
catch(e){
var _104="";
_104="";
_104+="Telerik RadGrid Error:\r\n";
_104+="-----------------\r\n";
_104+="Message: \""+e.message+"\"\r\n";
_104+="Raised by: "+_102.Type+"\r\n";
alert(_104);
}
}else{
if("function"==typeof (_103)){
try{
_103(this);
}
catch(e){
var _104="";
_104="";
_104+="Telerik RadGrid Error:\r\n";
_104+="-----------------\r\n";
_104+="Message: \""+e.message+"\"\r\n";
_104+="Raised by: "+_102.Type+"\r\n";
alert(_104);
}
}
}
}else{
this.Owner=_102;
for(var _105 in _101){
this[_105]=_101[_105];
}
this.Message="";
this.Message+="Telerik RadGrid Error:\r\n";
this.Message+="-----------------\r\n";
this.Message+="Message: \""+_101.message+"\"\r\n";
this.Message+="Raised by: "+_102.Type+"\r\n";
alert(this.Message);
}
};
RadGridNamespace.RadGrid.prototype.GetTableObjectByID=function(id){
if(this.MasterTableView.ClientID==id||this.MasterTableView.UID==id){
return this.MasterTableView;
}else{
for(var i=0;i<this.DetailTablesCollection.length;i++){
if(this.DetailTablesCollection[i].ClientID==id||this.DetailTablesCollection[i].UID==id){
return this.DetailTablesCollection[i];
}
}
}
if(this.MasterTableViewHeader!=null){
if(this.MasterTableViewHeader.ClientID==id||this.MasterTableViewHeader.UID==id){
return table=this.MasterTableViewHeader;
}
}
};
RadGridNamespace.RadGrid.prototype.GetRowObjectByRealRow=function(_108,row){
if(_108.Rows!=null){
for(var i=0;i<_108.Rows.length;i++){
if(_108.Rows[i].Control==row){
return _108.Rows[i];
}
}
}
};
RadGridNamespace.RadGrid.prototype.SavePostData=function(){
try{
var _10b=new String();
for(var i=0;i<arguments.length;i++){
_10b+=arguments[i]+",";
}
_10b=_10b.substring(0,_10b.length-1);
if(this.PostDataValue!=null){
switch(arguments[0]){
case "ReorderedColumns":
this.PostDataValue.value+=_10b+";";
break;
case "HidedColumns":
var _10d=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
_10d="ShowedColumns"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
break;
case "ShowedColumns":
var _10d=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
_10d="HidedColumns"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
break;
case "HidedRows":
var _10d=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
_10d="ShowedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
break;
case "ShowedRows":
var _10d=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
_10d="HidedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
break;
case "ResizedColumns":
var _10d=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
break;
case "ResizedRows":
var _10d=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
break;
case "ResizedControl":
var _10d=arguments[0]+","+arguments[1];
this.UpdatePostData(_10b,_10d);
break;
case "ClientCreated":
var _10d=arguments[0]+","+arguments[1];
this.UpdatePostData(_10b,_10d);
break;
case "ScrolledControl":
var _10d=arguments[0]+","+arguments[1];
this.UpdatePostData(_10b,_10d);
break;
case "AJAXScrolledControl":
var _10d=arguments[0]+","+arguments[1];
this.UpdatePostData(_10b,_10d);
break;
case "SelectedRows":
var _10d=arguments[0]+","+arguments[1]+",";
this.UpdatePostData(_10b,_10d);
break;
case "EditRow":
var _10d=arguments[0]+","+arguments[1];
this.UpdatePostData(_10b,_10d);
break;
case "ActiveRow":
var _10d=arguments[0]+","+arguments[1];
this.UpdatePostData(_10b,_10d);
break;
case "CollapsedRows":
var _10d=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
_10d="ExpandedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
break;
case "ExpandedRows":
var _10d=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
_10d="CollapsedRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
break;
case "CollapsedGroupRows":
var _10d=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
_10d="ExpandedGroupRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
break;
case "ExpandedGroupRows":
var _10d=arguments[0]+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
_10d="CollapsedGroupRows"+","+arguments[1]+","+arguments[2];
this.UpdatePostData(_10b,_10d);
break;
default:
this.UpdatePostData(_10b,_10b);
break;
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.UpdatePostData=function(_10e,_10f){
var _110,_111=new Array();
_110=this.PostDataValue.value.split(";");
for(var i=0;i<_110.length;i++){
if(_110[i].indexOf(_10f)==-1){
_111[_111.length]=_110[i];
}
}
this.PostDataValue.value=_111.join(";");
this.PostDataValue.value+=_10e+";";
};
RadGridNamespace.RadGrid.prototype.DeletePostData=function(_113,_114){
var _115,_116=new Array();
_115=this.PostDataValue.value.split(";");
for(var i=0;i<_115.length;i++){
if(_115[i].indexOf(_114)==-1){
_116[_116.length]=_115[i];
}
}
this.PostDataValue.value=_116.join(";");
};
RadGridNamespace.RadGrid.prototype.HandleDragAndDrop=function(e,_119){
try{
var _11a=this;
if(_11b!=null&&_11b.Columns.length>0&&_11b.Columns[_11c]!=null&&!_11b.Columns[_11c].Reorderable){
this.Control.style.cursor="no-drop";
this.DisableDrop();
}else{
this.Control.style.cursor="";
}
if(this.MoveHeaderDiv!=null&&_119!=null&&_119.tagName.toLowerCase()!="th"&&!RadGridNamespace.IsChildOf(_119,this.MoveHeaderDivRefCell.parentNode)&&!(this.GroupPanelControl!=null&&RadGridNamespace.IsChildOf(_119,this.GroupPanelControl))){
this.Control.style.cursor="no-drop";
this.DisableDrop();
}else{
this.Control.style.cursor="";
}
if((_119!=null)&&(_119.tagName.toLowerCase()=="th")){
var _11d=_119.parentNode.parentNode.parentNode;
var _11b=this.GetTableObjectByID(_11d.id);
var _11c=RadGridNamespace.GetRealCellIndex(_11b,_119);
if((_11b!=null)&&(_11b.Columns.length>0)&&(_11b.Columns[_11c]!=null)&&((_11b.Columns[_11c].Reorderable)||(_11b.Owner.ClientSettings.AllowDragToGroup&&_11b.Columns[_11c].Groupable))){
var _11e=RadGridNamespace.GetEventPosX(e);
var _11f=RadGridNamespace.FindPosX(_119);
var endX=_11f+_119.offsetWidth;
this.ResizeTolerance=5;
var _121=_119.title;
var _122=_119.style.cursor;
if(!((_11e>=endX-this.ResizeTolerance)&&(_11e<=endX+this.ResizeTolerance))){
if(this.MoveHeaderDiv){
if(this.MoveHeaderDiv.innerHTML!=_119.innerHTML){
_119.title=this.ClientSettings.ClientMessages.DropHereToReorder;
_119.style.cursor="default";
if(_119.parentNode.parentNode.parentNode==this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode){
this.MoveReorderIndicators(e,_119);
}else{
this.DisableDrop();
}
}
}else{
_119.title=this.ClientSettings.ClientMessages.DragToGroupOrReorder;
_119.style.cursor="move";
}
this.AttachDomEvent(_119,"mousedown","OnDragDropMouseDown");
}else{
_119.style.cursor=_122;
_119.title="";
}
}
}
if(_11b!=null&&_11b.Columns.length>0&&_11b.Columns[_11c]!=null&&!_11b.Columns[_11c].Reorderable){
this.Control.style.cursor="no-drop";
this.DisableDrop();
}
if(this.MoveHeaderDiv!=null){
this.MoveHeaderDiv.style.visibility="";
this.MoveHeaderDiv.style.display="";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.DisableDrop=function(e){
if(this.ReorderIndicator1!=null){
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
}
if(this.ReorderIndicator2!=null){
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
}
};
RadGridNamespace.RadGrid.PositionDragElement=function(_124,_125){
_124.style.top=_125.clientY+document.documentElement.scrollTop+document.body.scrollTop+1+"px";
_124.style.left=_125.clientX+document.documentElement.scrollLeft+document.body.scrollLeft+1+"px";
};
RadGridNamespace.RadGrid.prototype.OnDragDropMouseDown=function(e){
var _127=RadGridNamespace.GetCurrentElement(e);
var _128=false;
var form=document.getElementById(this.FormID);
if(form!=null&&form["__EVENTTARGET"]!=null&&form["__EVENTTARGET"].value!=""){
_128=true;
}
if((_127.tagName.toLowerCase()=="input"&&_127.type.toLowerCase()=="text")||(_127.tagName.toLowerCase()=="textarea")){
return;
}
_127=RadGridNamespace.GetFirstParentByTagName(_127,"th");
if(_127.tagName.toLowerCase()=="th"&&!this.IsResize){
if(((window.netscape||window.opera||navigator.userAgent.indexOf("Safari")!=-1)&&(e.button==0))||(e.button==1)){
this.CreateDragAndDrop(e,_127);
}
RadGridNamespace.ClearDocumentEvents();
this.DetachDomEvent(_127,"mousedown","OnDragDropMouseDown");
this.AttachDomEvent(document,"mouseup","OnDragDropMouseUp");
if(this.GroupPanelControl!=null){
this.AttachDomEvent(this.GroupPanelControl,"mouseup","OnDragDropMouseUp");
}
}
};
RadGridNamespace.RadGrid.prototype.OnDragDropMouseUp=function(e){
this.DetachDomEvent(document,"mouseup","OnDragDropMouseUp");
if(this.GroupPanelControl!=null){
this.DetachDomEvent(this.GroupPanelControl,"mouseup","OnDragDropMouseUp");
}
this.FireDropAction(e);
this.DestroyDragAndDrop(e);
RadGridNamespace.RestoreDocumentEvents();
};
RadGridNamespace.CopyAttributes=function(_12b,_12c){
for(var i=0;i<_12c.attributes.length;i++){
try{
if(_12c.attributes[i].name.toLowerCase()=="id"){
continue;
}
if(_12c.attributes[i].value!=null&&_12c.attributes[i].value!="null"&&_12c.attributes[i].value!=""){
_12b.setAttribute(_12c.attributes[i].name,_12c.attributes[i].value);
}
}
catch(e){
continue;
}
}
};
RadGridNamespace.RadGrid.prototype.CreateDragAndDrop=function(e,_12f){
this.MoveHeaderDivRefCell=_12f;
this.MoveHeaderDiv=document.createElement("div");
var _130=document.createElement("table");
if(this.MoveHeaderDiv.mergeAttributes){
this.MoveHeaderDiv.mergeAttributes(this.Control);
}else{
RadGridNamespace.CopyAttributes(this.MoveHeaderDiv,this.Control);
}
if(_130.mergeAttributes){
_130.mergeAttributes(this.MasterTableView.Control);
}else{
RadGridNamespace.CopyAttributes(_130,this.MasterTableView.Control);
}
_130.style.margin="0px";
_130.style.height=_12f.offsetHeight+"px";
_130.style.width=_12f.offsetWidth+"px";
var _131=document.createElement("thead");
var tr=document.createElement("tr");
_130.appendChild(_131);
_131.appendChild(tr);
tr.appendChild(_12f.cloneNode(true));
this.MoveHeaderDiv.appendChild(_130);
document.body.appendChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.style.height=_12f.offsetHeight+"px";
this.MoveHeaderDiv.style.width=_12f.offsetWidth+"px";
this.MoveHeaderDiv.style.position="absolute";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
if(window.netscape){
this.MoveHeaderDiv.style.MozOpacity=3/4;
}else{
this.MoveHeaderDiv.style.filter="alpha(opacity=75);";
}
this.MoveHeaderDiv.style.cursor="move";
this.MoveHeaderDiv.style.visibility="hidden";
this.MoveHeaderDiv.style.display="none";
this.MoveHeaderDiv.style.fontWeight="bold";
this.MoveHeaderDiv.onmousedown=null;
RadGridNamespace.ClearDocumentEvents();
if(this.ClientSettings.AllowColumnsReorder){
this.CreateReorderIndicators(_12f);
}
};
RadGridNamespace.RadGrid.prototype.DestroyDragAndDrop=function(){
if(this.MoveHeaderDiv!=null){
var _133=this.MoveHeaderDiv.parentNode;
_133.removeChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.onmouseup=null;
this.MoveHeaderDiv.onmousemove=null;
this.MoveHeaderDiv=null;
this.MoveHeaderDivRefCell=null;
this.DragCellIndex=null;
RadGridNamespace.RestoreDocumentEvents();
this.DestroyReorderIndicators();
}
};
RadGridNamespace.RadGrid.prototype.FireDropAction=function(e){
if((this.MoveHeaderDiv!=null)&&(this.MoveHeaderDiv.style.display!="none")){
var _135=RadGridNamespace.GetCurrentElement(e);
if((_135!=null)&&(this.MoveHeaderDiv!=null)){
if(_135!=this.MoveHeaderDivRefCell){
var _136=this.GetTableObjectByID(this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode.id);
var _137=_136.HeaderRow;
if(RadGridNamespace.IsChildOf(_135,_137)){
if(_135.tagName.toLowerCase()!="th"){
_135=RadGridNamespace.GetFirstParentByTagName(_135,"th");
}
var _138=_135.parentNode.parentNode.parentNode;
var _139=this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode;
if(_138.id==_139.id){
var _13a=this.GetTableObjectByID(_138.id);
var _13b=_135.cellIndex;
if(navigator.userAgent.indexOf("Safari")!=-1){
_13b=RadGridNamespace.GetRealCellIndex(_13a,_135);
}
var _13c=this.MoveHeaderDivRefCell.cellIndex;
if(navigator.userAgent.indexOf("Safari")!=-1){
_13c=RadGridNamespace.GetRealCellIndex(_13a,this.MoveHeaderDivRefCell);
}
if(!_13a||!_13a.Columns[_13b]){
return;
}
if(!_13a.Columns[_13b].Reorderable){
return;
}
_13a.SwapColumns(_13b,_13c,(this.ClientSettings.ColumnsReorderMethod!="Reorder"));
if(this.ClientSettings.ColumnsReorderMethod=="Reorder"){
if((!this.ClientSettings.ReorderColumnsOnClient)&&(this.ClientSettings.PostBackReferences.PostBackColumnsReorder!="")){
eval(this.ClientSettings.PostBackReferences.PostBackColumnsReorder);
}
}
}
}else{
if(RadGridNamespace.CheckParentNodesFor(_135,this.GroupPanelControl)){
if((this.ClientSettings.PostBackReferences.PostBackGroupByColumn!="")&&(this.ClientSettings.AllowDragToGroup)){
var _13a=this.GetTableObjectByID(this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode.id);
var _13d=this.MoveHeaderDivRefCell.cellIndex;
_13d=RadGridNamespace.GetRealCellIndex(_13a,this.MoveHeaderDivRefCell);
var _13e=_13a.Columns[_13d].RealIndex;
if(_13a.Columns[_13d].Groupable){
if(_13a==this.MasterTableViewHeader){
this.SavePostData("GroupByColumn",this.MasterTableView.ClientID,_13e);
}else{
this.SavePostData("GroupByColumn",_13a.ClientID,_13e);
}
eval(this.ClientSettings.PostBackReferences.PostBackGroupByColumn);
}
}
}
}
}
}
}
};
RadGridNamespace.GetRealCellIndex=function(_13f,cell){
for(var i=0;i<_13f.Columns.length;i++){
if(_13f.Columns[i].Control==cell){
return i;
}
}
};
RadGridNamespace.RadGrid.prototype.CreateReorderIndicators=function(_142){
if((this.ReorderIndicator1==null)&&(this.ReorderIndicator2==null)){
var _143=this.MoveHeaderDivRefCell.parentNode.parentNode.parentNode;
var _144=this.GetTableObjectByID(_143.id);
var _145=_144.HeaderRow;
if(!RadGridNamespace.IsChildOf(_142,_145)){
return;
}
this.ReorderIndicator1=document.createElement("span");
this.ReorderIndicator2=document.createElement("span");
if(this.Skin==""||this.Skin=="None"){
this.ReorderIndicator1.innerHTML="&darr;";
this.ReorderIndicator2.innerHTML="&uarr;";
}else{
this.ReorderIndicator1.className="TopReorderIndicator_"+this.Skin;
this.ReorderIndicator2.className="BottomReorderIndicator_"+this.Skin;
this.ReorderIndicator1.style.width=this.ReorderIndicator1.style.height=this.ReorderIndicator2.style.width=this.ReorderIndicator2.style.height="10px";
}
this.ReorderIndicator1.style.backgroundColor="transparent";
this.ReorderIndicator1.style.color="darkblue";
this.ReorderIndicator1.style.font="bold 18px Arial";
this.ReorderIndicator2.style.backgroundColor=this.ReorderIndicator1.style.backgroundColor;
this.ReorderIndicator2.style.color=this.ReorderIndicator1.style.color;
this.ReorderIndicator2.style.font=this.ReorderIndicator1.style.font;
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_142)-this.ReorderIndicator1.offsetHeight+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_142)+"px";
this.ReorderIndicator2.style.top=RadGridNamespace.FindPosY(_142)+_142.offsetHeight+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
document.body.appendChild(this.ReorderIndicator1);
document.body.appendChild(this.ReorderIndicator2);
}
};
RadGridNamespace.RadGrid.prototype.DestroyReorderIndicators=function(){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
document.body.removeChild(this.ReorderIndicator1);
document.body.removeChild(this.ReorderIndicator2);
this.ReorderIndicator1=null;
this.ReorderIndicator2=null;
}
};
RadGridNamespace.RadGrid.prototype.MoveReorderIndicators=function(e,_147){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
this.ReorderIndicator1.style.visibility="visible";
this.ReorderIndicator1.style.display="";
this.ReorderIndicator2.style.visibility="visible";
this.ReorderIndicator2.style.display="";
var _148=0;
if(document.body.currentStyle&&document.body.currentStyle.margin&&document.body.currentStyle.margin.indexOf("px")!=-1){
_148=parseInt(document.body.currentStyle.marginTop);
}
_148-=(this.Skin==""||this.Skin=="None"&&navigator.userAgent.indexOf("Safari")==-1)?10:5;
if(navigator.userAgent.indexOf("Safari")!=-1){
_148-=10;
}
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_147)-RadGridNamespace.FindScrollPosY(_147)+(document.documentElement.scrollTop+document.body.scrollTop)-this.ReorderIndicator1.offsetHeight/2+_148+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_147)-RadGridNamespace.FindScrollPosX(_147)+(document.documentElement.scrollLeft+document.body.scrollLeft)-document.documentElement.scrollLeft+document.body.scrollLeft+"px";
if(parseInt(this.ReorderIndicator1.style.left)<RadGridNamespace.FindPosX(this.Control)){
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(this.Control)+5;
}
this.ReorderIndicator2.style.top=parseInt(this.ReorderIndicator1.style.top)+_147.offsetHeight+this.ReorderIndicator2.offsetHeight+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
this.ReorderIndicator2.style.zIndex=this.ReorderIndicator1.style.zIndex=99999;
}
};
RadGridNamespace.RadGrid.prototype.AttachDomEvents=function(){
try{
this.AttachDomEvent(this.Control,"mousemove","OnMouseMove");
this.AttachDomEvent(this.Control,"keydown","OnKeyDown");
this.AttachDomEvent(this.Control,"keyup","OnKeyUp");
this.AttachDomEvent(this.Control,"click","OnClick");
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.OnMouseMove=function(e){
var _14a=RadGridNamespace.GetCurrentElement(e);
if(this.ClientSettings&&this.ClientSettings.Resizing.AllowRowResize){
this.DetectResizeCursorsOnRows(e,_14a);
this.MoveRowResizer(e);
}
if(this.ClientSettings&&(this.ClientSettings.AllowDragToGroup||this.ClientSettings.AllowColumnsReorder)){
this.HandleDragAndDrop(e,_14a);
}
};
RadGridNamespace.RadGrid.prototype.OnKeyDown=function(e){
var _14c={KeyCode:e.keyCode,IsShiftPressed:e.shiftKey,IsCtrlPressed:e.ctrlKey,IsAltPressed:e.altKey,Event:e};
if(!RadGridNamespace.FireEvent(this,"OnKeyPress",[_14c])){
return;
}
if(e.keyCode==16){
this.IsShiftPressed=true;
}
if(e.keyCode==17){
this.IsCtrlPressed=true;
}
if(this.ClientSettings&&this.ClientSettings.AllowKeyboardNavigation&&this.ActiveRow){
this.ActiveRow.HandleActiveRow(e);
}
if(e.keyCode==27&&this.MoveHeaderDiv){
this.DestroyDragAndDrop();
}
};
RadGridNamespace.RadGrid.prototype.OnClick=function(e){
};
RadGridNamespace.RadGrid.prototype.OnKeyUp=function(e){
if(e.keyCode==16){
this.IsShiftPressed=false;
}
if(e.keyCode==17){
this.IsCtrlPressed=false;
}
};
RadGridNamespace.RadGrid.prototype.DetectResizeCursorsOnRows=function(e,_150){
try{
var _151=this;
if((_150!=null)&&(_150.tagName.toLowerCase()=="td")&&!this.MoveHeaderDiv){
var _152=_150.parentNode.parentNode.parentNode;
var _153=this.GetTableObjectByID(_152.id);
if(_153!=null){
if(_153.Columns!=null){
if(_153.Columns[_150.cellIndex].ColumnType!="GridRowIndicatorColumn"){
return;
}
}
if(!_153.Control.tBodies[0]){
return;
}
var _154=this.GetRowObjectByRealRow(_153,_150.parentNode);
if(_154!=null){
var _155=RadGridNamespace.GetEventPosY(e);
var _156=RadGridNamespace.FindPosY(_150);
var endY=_156+_150.offsetHeight;
this.ResizeTolerance=5;
var _158=_150.title;
if((_155>endY-this.ResizeTolerance)&&(_155<endY+this.ResizeTolerance)){
_150.style.cursor="n-resize";
_150.title=this.ClientSettings.ClientMessages.DragToResize;
this.AttachDomEvent(_150,"mousedown","OnResizeMouseDown");
}else{
_150.style.cursor="default";
_150.title="";
this.DetachDomEvent(_150,"mousedown","OnResizeMouseDown");
}
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.OnResizeMouseDown=function(e){
this.CreateRowResizer(e);
RadGridNamespace.ClearDocumentEvents();
this.AttachDomEvent(document,"mouseup","OnResizeMouseUp");
};
RadGridNamespace.RadGrid.prototype.OnResizeMouseUp=function(e){
this.DetachDomEvent(document,"mouseup","OnResizeMouseUp");
this.DestroyRowResizerAndResizeRow(e,true);
RadGridNamespace.RestoreDocumentEvents();
};
RadGridNamespace.RadGrid.prototype.CreateRowResizer=function(e){
try{
this.DestroyRowResizer();
var _15c=RadGridNamespace.GetCurrentElement(e);
if((_15c!=null)&&(_15c.tagName.toLowerCase()=="td")){
if(_15c.cellIndex>0){
var _15d=_15c.parentNode.rowIndex;
_15c=_15c.parentNode.parentNode.parentNode.rows[_15d].cells[0];
}
this.RowResizer=null;
this.CellToResize=_15c;
var _15e=_15c.parentNode.parentNode.parentNode;
var _15f=this.GetTableObjectByID(_15e.id);
this.RowResizer=document.createElement("div");
this.RowResizer.style.backgroundColor="navy";
this.RowResizer.style.height="1px";
this.RowResizer.style.fontSize="1";
this.RowResizer.style.position="absolute";
this.RowResizer.style.cursor="n-resize";
if(_15f!=null){
this.RowResizerRefTable=_15f;
if(this.GridDataDiv){
this.RowResizer.style.left=RadGridNamespace.FindPosX(this.GridDataDiv)+"px";
var _160=(RadGridNamespace.FindPosX(this.GridDataDiv)+this.GridDataDiv.offsetWidth)-parseInt(this.RowResizer.style.left);
if(_160>_15f.Control.offsetWidth){
this.RowResizer.style.width=_15f.Control.offsetWidth+"px";
}else{
this.RowResizer.style.width=_160+"px";
}
if(parseInt(this.RowResizer.style.width)>this.GridDataDiv.offsetWidth){
this.RowResizer.style.width=this.GridDataDiv.offsetWidth+"px";
}
}else{
this.RowResizer.style.width=_15f.Control.offsetWidth+"px";
this.RowResizer.style.left=RadGridNamespace.FindPosX(_15c)+"px";
}
}
this.RowResizer.style.top=RadGridNamespace.GetEventPosY(e)-(RadGridNamespace.GetEventPosY(e)-e.clientY)+document.body.scrollTop+document.documentElement.scrollTop+"px";
var _161=document.body;
_161.appendChild(this.RowResizer);
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.DestroyRowResizerAndResizeRow=function(e,_163){
try{
if((this.CellToResize!="undefined")&&(this.CellToResize!=null)&&(this.CellToResize.tagName.toLowerCase()=="td")&&(this.RowResizer!="undefined")&&(this.RowResizer!=null)){
var _164;
if(this.GridDataDiv){
_164=parseInt(this.RowResizer.style.top)+this.GridDataDiv.scrollTop-(RadGridNamespace.FindPosY(this.CellToResize));
}else{
_164=parseInt(this.RowResizer.style.top)-(RadGridNamespace.FindPosY(this.CellToResize));
}
if(_164>0){
var _165=this.CellToResize.parentNode.parentNode.parentNode;
var _166=this.GetTableObjectByID(_165.id);
if(_166!=null){
_166.ResizeRow(this.CellToResize.parentNode.rowIndex,_164);
}
}
}
if(_163){
this.DestroyRowResizer();
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.DestroyRowResizer=function(){
try{
if((this.RowResizer!="undefined")&&(this.RowResizer!=null)&&(this.RowResizer.parentNode!=null)){
var _167=this.RowResizer.parentNode;
_167.removeChild(this.RowResizer);
this.RowResizer=null;
this.RowResizerRefTable=null;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.MoveRowResizer=function(e){
try{
if((this.RowResizer!="undefined")&&(this.RowResizer!=null)&&(this.RowResizer.parentNode!=null)){
this.RowResizer.style.top=RadGridNamespace.GetEventPosY(e)-(RadGridNamespace.GetEventPosY(e)-e.clientY)+document.body.scrollTop+document.documentElement.scrollTop+"px";
if(this.ClientSettings.Resizing.EnableRealTimeResize){
this.DestroyRowResizerAndResizeRow(e,false);
this.UpdateRowResizerWidth(e);
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.UpdateRowResizerWidth=function(e){
var _16a=RadGridNamespace.GetCurrentElement(e);
if((_16a!=null)&&(_16a.tagName.toLowerCase()=="td")){
var _16b=this.RowResizerRefTable;
if(_16b!=null){
if(this.GridDataDiv){
var _16c=(RadGridNamespace.FindPosX(this.GridDataDiv)+this.GridDataDiv.offsetWidth)-parseInt(this.RowResizer.style.left);
if(_16c>_16b.Control.offsetWidth){
this.RowResizer.style.width=_16b.Control.offsetWidth+"px";
}else{
this.RowResizer.style.width=_16c+"px";
}
if(parseInt(this.RowResizer.style.width)>this.GridDataDiv.offsetWidth){
this.RowResizer.style.width=this.GridDataDiv.offsetWidth+"px";
}
}else{
this.RowResizer.style.width=_16b.Control.offsetWidth+"px";
}
}
}
};
RadGridNamespace.RadGrid.prototype.SetHeaderAndFooterDivsWidth=function(){
if((document.compatMode=="BackCompat"&&navigator.userAgent.toLowerCase().indexOf("msie")!=-1)||(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&navigator.userAgent.toLowerCase().indexOf("6.0")!=-1)){
if(this.ClientSettings.Scrolling.UseStaticHeaders){
if(this.GridHeaderDiv!=null&&this.GridDataDiv!=null&&this.GridHeaderDiv!=null){
this.GridHeaderDiv.style.width="100%";
if(this.GridHeaderDiv&&this.GridDataDiv){
if(this.GridDataDiv.offsetWidth>0){
this.GridHeaderDiv.style.width=this.GridDataDiv.offsetWidth-RadGridNamespace.GetScrollBarHeight()+"px";
}
}
if(this.GridHeaderDiv&&this.GridFooterDiv){
this.GridFooterDiv.style.width=this.GridHeaderDiv.style.width;
}
}
}
}
if(this.ClientSettings.Scrolling.AllowScroll&&this.ClientSettings.Scrolling.UseStaticHeaders){
var _16d=RadGridNamespace.IsRightToLeft(this.GridHeaderDiv);
if((!_16d&&this.GridHeaderDiv&&parseInt(this.GridHeaderDiv.style.marginRight)!=RadGridNamespace.GetScrollBarHeight())||(_16d&&this.GridHeaderDiv&&parseInt(this.GridHeaderDiv.style.marginLeft)!=RadGridNamespace.GetScrollBarHeight())){
if(!_16d){
this.GridHeaderDiv.style.marginRight=RadGridNamespace.GetScrollBarHeight()+"px";
this.GridHeaderDiv.style.marginLeft="";
}else{
this.GridHeaderDiv.style.marginLeft=RadGridNamespace.GetScrollBarHeight()+"px";
this.GridHeaderDiv.style.marginRight="";
}
}
if(this.GridHeaderDiv&&this.GridDataDiv){
if((this.GridDataDiv.clientWidth==this.GridDataDiv.offsetWidth)){
this.GridHeaderDiv.style.width="100%";
if(!_16d){
this.GridHeaderDiv.style.marginRight="";
}else{
this.GridHeaderDiv.style.marginLeft="";
}
}
}
if(this.GroupPanelObject&&this.GroupPanelObject.Items.length>0&&navigator.userAgent.toLowerCase().indexOf("msie")!=-1){
if(this.MasterTableView&&this.MasterTableViewHeader){
this.MasterTableView.Control.style.width=this.MasterTableViewHeader.Control.offsetWidth+"px";
}
}
if(this.GridFooterDiv){
this.GridFooterDiv.style.marginRight=this.GridHeaderDiv.style.marginRight;
this.GridFooterDiv.style.marginLeft=this.GridHeaderDiv.style.marginLeft;
this.GridFooterDiv.style.width=this.GridHeaderDiv.style.width;
}
}
};
RadGridNamespace.RadGrid.prototype.SetDataDivHeight=function(){
if(this.GridDataDiv&&this.Control.style.height!=""){
this.GridDataDiv.style.height="10px";
var _16e=0;
if(this.GroupPanelObject){
_16e+=this.GroupPanelObject.Control.offsetHeight;
}
if(this.GridHeaderDiv){
_16e+=this.GridHeaderDiv.offsetHeight;
}
if(this.GridFooterDiv){
_16e+=this.GridFooterDiv.offsetHeight;
}
if(this.PagerControl){
_16e+=this.PagerControl.offsetHeight;
}
if(this.TopPagerControl){
_16e+=this.TopPagerControl.offsetHeight;
}
if(this.ClientSettings.Scrolling.FrozenColumnsCount>0){
_16e+=RadGridNamespace.GetScrollBarHeight();
}
var _16f=this.Control.clientHeight-_16e;
if(_16f>0){
var _170=this.Control.style.position;
if(window.netscape){
this.Control.style.position="absolute";
}
this.GridDataDiv.style.height=_16f+"px";
if(window.netscape){
this.Control.style.position=_170;
}
}
}
};
RadGridNamespace.RadGrid.prototype.InitializeDimensions=function(){
try{
var _171=this;
this.InitializeAutoLayout();
this.ApplyFrozenScroll();
if(!this.EnableAJAX){
this.OnWindowResize();
}else{
var _172=function(){
_171.OnWindowResize();
};
if(window.netscape&&!window.opera){
_172();
}else{
setTimeout(_172,0);
}
}
this.Control.RadResize=function(){
_171.OnWindowResize();
};
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1){
setTimeout(function(){
_171.AttachDomEvent(window,"resize","OnWindowResize");
},0);
}else{
this.AttachDomEvent(window,"resize","OnWindowResize");
}
this.Control.RadShow=function(){
_171.OnWindowResize();
};
if(this.ClientSettings.Scrolling.FrozenColumnsCount>0){
this.ClientSettings.Scrolling.FrozenColumnsCount+=this.MasterTableViewHeader.ExpandCollapseColumns.length+this.MasterTableViewHeader.GroupSplitterColumns.length;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.OnError);
}
};
RadGridNamespace.RadGrid.prototype.OnWindowResize=function(e){
this.InitializeAutoLayout();
this.SetHeaderAndFooterDivsWidth();
this.SetDataDivHeight();
};
RadGridNamespace.RadGrid.prototype.InitializeAutoLayout=function(){
if(this.ClientSettings.Scrolling.AllowScroll&&this.ClientSettings.Scrolling.UseStaticHeaders){
if(this.MasterTableView&&this.MasterTableViewHeader){
if(this.MasterTableView.TableLayout!="Auto"||window.netscape||window.opera){
return;
}
this.MasterTableView.Control.style.tableLayout=this.MasterTableViewHeader.Control.style.tableLayout="";
if(this.MasterTableViewFooter&&this.MasterTableViewFooter.Control){
this.MasterTableViewFooter.Control.style.tableLayout="";
}
if(this.MasterTableView.Control.tBodies.length>0){
var _174=this.MasterTableView.Control.tBodies[0].rows[this.ClientSettings.FirstDataRowClientRowIndex];
var _175=this.MasterTableViewHeader.HeaderRow;
var _176=0;
for(var i=0;i<_174.cells.length;i++){
if(parseInt(_174.cells[i].colSpan)>1){
_176+=parseInt(_174.cells[i].colSpan);
}
}
var _178=Math.min(_175.cells.length,(_174.cells.length+(_176-1)));
var _179=0;
var _17a=0;
for(var i=0;i<_178+1;i++){
var col=this.MasterTableViewHeader.ColGroup.Cols[i];
if(!col){
continue;
}
if(col.width!=""){
continue;
}
var _17c=_175.cells[i].offsetWidth;
var _17d=0;
if(_17a>0){
_17d=_17c;
_17a--;
}else{
_17d=_174.cells[i].offsetWidth;
if(_174.cells[i].colSpan>1){
_17a=_174.cells[i].colSpan;
_17d=_175.cells[i].offsetWidth;
_17a--;
}
}
var _17e=(_17c>_17d)?_17c:_17d;
if(this.MasterTableViewFooter&&this.MasterTableViewFooter.Control){
if(this.MasterTableViewFooter.Control.tBodies[0].rows[0]&&this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i]){
if(this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i].offsetWidth>_17e){
_17e=this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i].offsetWidth;
}
}
}
if(_17e<=0){
continue;
}
_175.cells[i].style.width=_17e;
if(_17a==0){
_174.cells[i].style.width=_17e;
this.MasterTableView.ColGroup.Cols[i].width=col.width=_17e;
}
if(this.MasterTableViewFooter&&this.MasterTableViewFooter.Control){
if(this.MasterTableViewFooter.Control.tBodies[0].rows[0]&&this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i]){
this.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[i].style.width=_17e;
}
}
}
}
this.MasterTableView.Control.style.tableLayout=this.MasterTableViewHeader.Control.style.tableLayout="fixed";
if(this.MasterTableViewFooter&&this.MasterTableViewFooter.Control){
this.MasterTableViewFooter.Control.style.tableLayout="fixed";
}
if(window.netscape){
this.OnWindowResize();
}
}
}
};
RadGridNamespace.RadGrid.prototype.InitializeSaveScrollPosition=function(){
if(!this.ClientSettings.Scrolling.SaveScrollPosition){
return;
}
if(this.ClientSettings.Scrolling.ScrollTop!=""&&!this.ClientSettings.Scrolling.EnableAJAXScrollPaging){
this.GridDataDiv.scrollTop=this.ClientSettings.Scrolling.ScrollTop;
}
if(this.ClientSettings.Scrolling.ScrollLeft!=""){
var _17f=document.getElementById(this.ClientID+"_Frozen");
if(this.GridHeaderDiv&&!_17f){
this.GridHeaderDiv.scrollLeft=this.ClientSettings.Scrolling.ScrollLeft;
}
if(this.GridFooterDiv&&!_17f){
this.GridFooterDiv.scrollLeft=this.ClientSettings.Scrolling.ScrollLeft;
}
if(_17f){
_17f.scrollLeft=this.ClientSettings.Scrolling.ScrollLeft;
}else{
this.GridDataDiv.scrollLeft=this.ClientSettings.Scrolling.ScrollLeft;
}
}
};
RadGridNamespace.RadGrid.prototype.InitializeAjaxScrollPaging=function(){
if(!this.ClientSettings.Scrolling.EnableAJAXScrollPaging){
return;
}
this.ScrollCounter=0;
this.CurrentAJAXScrollTop=0;
if(this.ClientSettings.Scrolling.AJAXScrollTop!=""){
this.CurrentAJAXScrollTop=this.ClientSettings.Scrolling.AJAXScrollTop;
}
var _180=this.CurrentPageIndex*this.MasterTableView.PageSize*20;
var _181=this.MasterTableView.PageCount*this.MasterTableView.PageSize*20;
var _182=this.MasterTableView.Control;
var _183=_182.offsetHeight;
if(!window.opera){
_182.style.marginTop=_180+"px";
_182.style.marginBottom=_181-_180-_183+"px";
}else{
_182.style.position="relative";
_182.style.top=_180+"px";
_182.style.marginBottom=_181-_183+"px";
}
this.CurrentAJAXScrollTop=_180;
this.GridDataDiv.scrollTop=_180;
this.CreateScrollerToolTip();
this.AttachDomEvent(this.GridDataDiv,"scroll","OnAJAXScroll");
};
RadGridNamespace.RadGrid.prototype.CreateScrollerToolTip=function(){
var _184=document.getElementById(this.ClientID+"ScrollerToolTip");
if(!_184){
this.ScrollerToolTip=document.createElement("span");
this.ScrollerToolTip.id=this.ClientID+"ScrollerToolTip";
this.ScrollerToolTip.style.backgroundColor="#F5F5DC";
this.ScrollerToolTip.style.border="1px solid";
this.ScrollerToolTip.style.position="absolute";
this.ScrollerToolTip.style.display="none";
this.ScrollerToolTip.style.font="icon";
this.ScrollerToolTip.style.padding="2";
document.body.appendChild(this.ScrollerToolTip);
}
};
RadGridNamespace.RadGrid.prototype.HideScrollerToolTip=function(){
var _185=this;
setTimeout(function(){
var _186=document.getElementById(_185.ClientID+"ScrollerToolTip");
if(_186&&_186.parentNode){
_186.style.display="none";
}
},200);
};
RadGridNamespace.RadGrid.prototype.ShowScrollerTooltip=function(_187,_188){
var _189=document.getElementById(this.ClientID+"ScrollerToolTip");
if(_189){
_189.style.display="";
_189.style.top=parseInt(RadGridNamespace.FindPosY(this.GridDataDiv))+Math.round(this.GridDataDiv.offsetHeight*_187)+"px";
_189.style.left=parseInt(RadGridNamespace.FindPosX(this.GridDataDiv))+this.GridDataDiv.offsetWidth-(this.GridDataDiv.offsetWidth-this.GridDataDiv.clientWidth)-_189.offsetWidth+"px";
this.ApplyPagerTooltipText(_189,_188,this.MasterTableView.PageCount);
}
};
RadGridNamespace.RadGrid.prototype.ApplyPagerTooltipText=function(_18a,_18b,_18c){
var _18d=this.ClientSettings.ClientMessages.PagerTooltipFormatString;
var _18e=/\{0[^\}]*\}/g;
var _18f=/\{1[^\}]*\}/g;
var _190=((_18b==0)?1:_18b+1);
var _191=_18c;
_18d=_18d.replace(_18e,_190).replace(_18f,_191);
_18a.innerHTML=_18d;
};
RadGridNamespace.RadGrid.prototype.InitializeScroll=function(){
var _192=this;
var grid=this;
var _194=function(){
grid.InitializeSaveScrollPosition();
};
if((window.netscape&&!window.opera)||this.EnableAJAX){
window.setTimeout(_194,0);
}else{
_194();
}
this.InitializeAjaxScrollPaging();
this.AttachDomEvent(this.GridDataDiv,"scroll","OnGridScroll");
if(this.GridHeaderDiv){
this.AttachDomEvent(this.GridHeaderDiv,"scroll","OnGridScroll");
}
};
RadGridNamespace.RadGrid.prototype.ApplyFrozenScroll=function(){
this.isFrozenScroll=false;
if(this.MasterTableView.FrozenColumnsCount==0){
return;
}
var _195=document.getElementById(this.ClientID+"_Frozen");
var _196=RadGridNamespace.GetScrollBarHeight();
if(_195){
var _197=document.getElementById(this.ClientID+"_FrozenScroll");
this.AttachDomEvent(_195,"scroll","OnGridFrozenScroll");
if(this.MasterTableView.Control.offsetWidth>this.GridDataDiv.clientWidth){
_195.style.height=_196+"px";
_197.style.width=this.GridDataDiv.scrollWidth+"px";
_197.style.height=_196+"px";
if(this.ClientSettings.Scrolling.SaveScrollPosition&&this.ClientSettings.Scrolling.ScrollLeft!=""){
_195.scrollLeft=this.ClientSettings.Scrolling.ScrollLeft;
}
if(this.GridDataDiv.style.overflowX!=null){
this.GridDataDiv.style.overflowX="hidden";
}else{
_195.style.marginTop="-16px";
_195.style.zIndex=99999;
_195.style.position="relative";
}
if(window.netscape&&!window.opera){
_195.style.width=this.GridDataDiv.offsetWidth-_196+"px";
}
if(this.GridHeaderDiv&&this.GridDataDiv){
if((this.GridDataDiv.clientWidth==this.GridDataDiv.offsetWidth)){
if(typeof (_195.style.overflowX)!="undefined"&&typeof (_195.style.overflowY)!="undefined"){
_195.style.overflowX="auto";
_195.style.overflowY="hidden";
if(window.netscape){
_195.style.width=parseInt(_195.style.width)+_196+"px";
}
}
}
}
this.isFrozenScroll=true;
}else{
_195.style.height="";
_197.style.width="";
this.GridDataDiv.style.overflow="auto";
this.isFrozenScroll=false;
}
}
};
RadGridNamespace.RadGrid.prototype.OnGridFrozenScroll=function(e){
if(!this.FrozenScrollCounter){
this.FrozenScrollCounter=0;
}
this.FrozenScrollCounter++;
var _199=this;
_199.currentElement=(e.srcElement)?e.srcElement:e.target;
RadGridNamespace.FrozenScrollHanlder=function(_19a){
if(_199.FrozenScrollCounter!=_19a){
return;
}
if(!_199.LastScrollIndex){
_199.LastScrollIndex=0;
}
var _19b=_199.currentElement;
if(_199.ClientSettings.Scrolling.FrozenColumnsCount>_199.MasterTableViewHeader.Columns.length){
_199.isFrozenScroll=false;
}
if(_199.isFrozenScroll){
var _19c=_199.MasterTableView.Columns[_199.ClientSettings.Scrolling.FrozenColumnsCount-1].Control;
var _19d=RadGridNamespace.FindPosX(_19c)-RadGridNamespace.FindScrollPosX(_19c)+document.documentElement.scrollLeft+document.body.scrollLeft+_19c.offsetWidth;
var _19e=_19b.scrollWidth-_19d;
_199.notFrozenColumns=[];
var _19f=_199.MasterTableView.Control.tBodies[0].rows[_199.ClientSettings.FirstDataRowClientRowIndex];
for(var i=_199.ClientSettings.Scrolling.FrozenColumnsCount;i<_199.MasterTableView.Columns.length;i++){
var _1a1=_199.MasterTableView.Columns[i];
var _1a2=false;
if(window.netscape&&_1a1.Control.style.display=="none"){
_1a1.Control.style.display="table-cell";
_1a2=true;
}
var _1a3=(_1a1.Control.offsetWidth>0)?_1a1.Control.offsetWidth:_19f.cells[i].offsetWidth;
_199.notFrozenColumns[_199.notFrozenColumns.length]={Index:i,Width:_1a3};
if(window.netscape&&_1a2){
_1a1.Control.style.display="none";
_1a2=false;
}
}
var _1a4=RadGridNamespace.GetScrollBarHeight();
if(window.netscape&&!window.opera){
_1a4=0;
}
var _1a5=Math.floor(_19b.scrollLeft/(_19b.scrollWidth-(1.5*_19c.offsetWidth))*100);
var _1a6=0;
var i=0;
while(i<_199.notFrozenColumns.length-1){
var _1a1=_199.notFrozenColumns[i];
var _1a7=Math.floor(_1a1.Width/_19e*100);
if(_1a7+_1a6<_1a5){
if(typeof (_199.MasterTableView.Columns[_1a1.Index].FrozenDisplay)=="boolean"&&!_199.MasterTableView.Columns[_1a1.Index].FrozenDisplay){
i++;
continue;
}
_199.MasterTableViewHeader.HideNotFrozenColumn(_1a1.Index);
_1a6+=_1a7;
}else{
if(typeof (_199.MasterTableView.Columns[_1a1.Index].FrozenDisplay)=="boolean"&&_199.MasterTableView.Columns[_1a1.Index].FrozenDisplay){
i++;
continue;
}
_199.MasterTableViewHeader.ShowNotFrozenColumn(_1a1.Index);
}
i++;
}
_199.MasterTableView.Control.style.width=_199.MasterTableViewHeader.Control.offsetWidth+"px";
if(_199.MasterTableViewFooter){
_199.MasterTableViewFooter.Control.style.width=_199.MasterTableViewHeader.Control.offsetWidth+"px";
}
_199.SavePostData("ScrolledControl",_199.ClientID,_199.GridDataDiv.scrollTop,_19b.scrollLeft);
}else{
_199.GridDataDiv.scrollLeft=_19b.scrollLeft;
}
_199.FrozenScrollCounter=0;
};
setTimeout("RadGridNamespace.FrozenScrollHanlder("+this.FrozenScrollCounter+")",100);
};
RadGridNamespace.RadGrid.prototype.OnGridScroll=function(e){
var _1a9=(e.srcElement)?e.srcElement:e.target;
if(window.opera&&this.isFrozenScroll){
this.GridDataDiv.scrollLeft=this.GridHeaderDiv.scrollLeft=0;
return;
}
if(this.ClientSettings.Scrolling.UseStaticHeaders){
if(navigator.userAgent.indexOf("Safari")!=-1){
var obj=this;
window.setTimeout(function(){
obj.UpdateDataDivScrollPos(_1a9);
},1);
}else{
this.UpdateDataDivScrollPos(_1a9);
}
}
this.SavePostData("ScrolledControl",this.ClientID,this.GridDataDiv.scrollTop,this.GridDataDiv.scrollLeft);
var evt={};
evt.ScrollTop=this.GridDataDiv.scrollTop;
evt.ScrollLeft=this.GridDataDiv.scrollLeft;
evt.ScrollControl=this.GridDataDiv;
evt.IsOnTop=(this.GridDataDiv.scrollTop==0)?true:false;
evt.IsOnBottom=((this.GridDataDiv.scrollHeight-this.GridDataDiv.offsetHeight+16)==this.GridDataDiv.scrollTop)?true:false;
RadGridNamespace.FireEvent(this,"OnScroll",[evt]);
};
RadGridNamespace.RadGrid.prototype.UpdateDataDivScrollPos=function(_1ac){
if(!_1ac){
return;
}
if(!this.isFrozenScroll){
if(this.GridHeaderDiv){
if(_1ac==this.GridHeaderDiv){
if(navigator.userAgent.indexOf("Safari")!=-1){
this.DetachDomEvent(this.GridDataDiv,"scroll","OnGridScroll");
this.GridDataDiv.scrollLeft=this.GridHeaderDiv.scrollLeft;
this.AttachDomEvent(this.GridDataDiv,"scroll","OnGridScroll");
}else{
this.GridDataDiv.scrollLeft=this.GridHeaderDiv.scrollLeft;
}
}
if(_1ac==this.GridDataDiv){
if(navigator.userAgent.indexOf("Safari")!=-1){
this.DetachDomEvent(this.GridHeaderDiv,"scroll","OnGridScroll");
this.GridHeaderDiv.scrollLeft=this.GridDataDiv.scrollLeft;
this.AttachDomEvent(this.GridHeaderDiv,"scroll","OnGridScroll");
}else{
this.GridHeaderDiv.scrollLeft=this.GridDataDiv.scrollLeft;
}
}
}
if(this.GridFooterDiv){
this.GridFooterDiv.scrollLeft=this.GridDataDiv.scrollLeft;
}
}else{
if(this.GridHeaderDiv){
if(navigator.userAgent.indexOf("Safari")!=-1){
this.DetachDomEvent(this.GridHeaderDiv,"scroll","OnGridScroll");
this.GridHeaderDiv.scrollLeft=this.GridDataDiv.scrollLeft;
this.AttachDomEvent(this.GridHeaderDiv,"scroll","OnGridScroll");
}else{
this.GridHeaderDiv.scrollLeft=this.GridDataDiv.scrollLeft;
}
}
if(this.GridFooterDiv){
this.GridFooterDiv.scrollLeft=this.GridDataDiv.scrollLeft;
}
}
};
RadGridNamespace.RadGrid.prototype.OnAJAXScroll=function(e){
if(this.GridDataDiv){
this.CurrentScrollTop=this.GridDataDiv.scrollTop;
}
this.ScrollCounter++;
var _1ae=this;
RadGridNamespace.AJAXScrollHanlder=function(_1af){
if(_1ae.ScrollCounter!=_1af){
return;
}
if(_1ae.CurrentAJAXScrollTop!=_1ae.GridDataDiv.scrollTop){
if(_1ae.CurrentPageIndex==_1b0){
return;
}
var _1b1=_1ae.ClientID;
var _1b2=_1ae.MasterTableView.ClientID;
_1ae.SavePostData("AJAXScrolledControl",_1ae.GridDataDiv.scrollLeft,_1ae.LastScrollTop,_1ae.GridDataDiv.scrollTop,_1b0);
var _1b3=_1ae.ClientSettings.PostBackFunction;
_1b3=_1b3.replace("{0}",_1ae.UniqueID);
eval(_1b3);
}
_1ae.ScrollCounter=0;
_1ae.HideScrollerToolTip();
};
var evt={};
evt.ScrollTop=this.GridDataDiv.scrollTop;
evt.ScrollLeft=this.GridDataDiv.scrollLeft;
evt.ScrollControl=this.GridDataDiv;
evt.IsOnTop=(this.GridDataDiv.scrollTop==0)?true:false;
evt.IsOnBottom=((this.GridDataDiv.scrollHeight-this.GridDataDiv.offsetHeight+16)==this.GridDataDiv.scrollTop)?true:false;
RadGridNamespace.FireEvent(this,"OnScroll",[evt]);
var _1b5=this.GridDataDiv.scrollTop/(this.GridDataDiv.scrollHeight-this.GridDataDiv.offsetHeight+16);
var _1b0=Math.round((this.MasterTableView.PageCount-1)*_1b5);
setTimeout("RadGridNamespace.AJAXScrollHanlder("+this.ScrollCounter+")",500);
this.ShowScrollerTooltip(_1b5,_1b0);
};
RadGridNamespace.RadGridTable=function(_1b6){
if((!_1b6)||typeof (_1b6)!="object"){
return;
}
for(var _1b7 in _1b6){
this[_1b7]=_1b6[_1b7];
}
this.Type="RadGridTable";
this.ServerID=this.ID;
this.SelectedRows=new Array();
this.SelectedCells=new Array();
this.SelectedColumns=new Array();
this.ExpandCollapseColumns=new Array();
this.GroupSplitterColumns=new Array();
this.HeaderRow=null;
};
RadGridNamespace.RadGridTable.prototype._constructor=function(_1b8){
if((!_1b8)||typeof (_1b8)!="object"){
return;
}
this.Control=document.getElementById(this.ClientID);
if(!this.Control){
return;
}
this.ColGroup=RadGridNamespace.GetTableColGroup(this.Control);
if(!this.ColGroup){
return;
}
this.ColGroup.Cols=RadGridNamespace.GetTableColGroupCols(this.ColGroup);
this.Owner=_1b8;
if(this.Owner.ClientSettings){
this.InitializeEvents(this.Owner.ClientSettings.ClientEvents);
this.Control.style.overflow=((this.Owner.ClientSettings.Resizing.ClipCellContentOnResize&&((this.Owner.ClientSettings.Resizing.AllowColumnResize)||(this.Owner.ClientSettings.Resizing.AllowRowResize)))||(this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders))?"hidden":"";
}
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&this.Control.style.tableLayout=="fixed"&&this.Control.style.width.indexOf("%")!=-1){
this.Control.style.width="";
}
this.CreateStyles();
if(this.Owner.ClientSettings&&this.Owner.ClientSettings.Scrolling.AllowScroll&&this.Owner.ClientSettings.Scrolling.UseStaticHeaders){
if(this.ClientID.indexOf("_Header")!=-1||this.ClientID.indexOf("_Detail")!=-1){
this.Columns=this.GetTableColumns(this.Control,this.RenderColumns);
}else{
this.Columns=this.Owner.MasterTableViewHeader.Columns;
this.ExpandCollapseColumns=this.Owner.MasterTableViewHeader.ExpandCollapseColumns;
this.GroupSplitterColumns=this.Owner.MasterTableViewHeader.GroupSplitterColumns;
}
}else{
this.Columns=this.GetTableColumns(this.Control,this.RenderColumns);
}
if(this.Owner.ClientSettings&&this.Owner.ClientSettings.ShouldCreateRows){
this.InitializeRows(this.Controls[0].Rows);
}
};
RadGridNamespace.RadGridTable.prototype.Dispose=function(){
if(this.ColGroup&&this.ColGroup.Cols){
this.ColGroup.Cols=null;
this.ColGroup=null;
}
this.Owner=null;
this.DisposeEvents();
this.ExpandCollapseColumns=null;
this.GroupSplitterColumns=null;
this.DisposeRows();
this.DisposeColumns();
this.RenderColumns=null;
this.SelectedRows=null;
this.ExpandCollapseColumns=null;
this.DetailTables=null;
this.DetailTablesCollection=null;
this.Control=null;
this.HeaderRow=null;
};
RadGridNamespace.RadGridTable.prototype.CreateStyles=function(){
if(!this.SelectedItemStyleClass||this.SelectedItemStyleClass==""){
if(this.SelectedItemStyle&&this.SelectedItemStyle!=""){
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".SelectedItemStyle"+this.ClientID+"1 td",this.SelectedItemStyle);
}else{
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".SelectedItemStyle"+this.ClientID+"2 td","background-color:Navy;color:White;");
}
}
RadGridNamespace.addClassName(this.Control,"grid"+this.ClientID);
if(window.netscape&&!window.opera){
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" td","-moz-user-select:-moz-none;");
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" th","-moz-user-select:-moz-none;");
}else{
if(window.opera||navigator.userAgent.indexOf("Safari")!=-1){
var _1b9=this;
setTimeout(function(){
RadGridNamespace.AddRule(_1b9.Owner.GridStyleSheet,".grid"+_1b9.ClientID+" td","overflow: hidden;");
RadGridNamespace.AddRule(_1b9.Owner.GridStyleSheet,".grid"+_1b9.ClientID+" th","overflow: hidden;");
},100);
}else{
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" td","overflow: hidden; text-overflow: ellipsis;");
RadGridNamespace.AddRule(this.Owner.GridStyleSheet,".grid"+this.ClientID+" th","overflow: hidden; text-overflow: ellipsis;");
}
}
};
RadGridNamespace.RadGridTable.prototype.InitializeEvents=function(_1ba){
for(clientEvent in _1ba){
if(typeof (_1ba[clientEvent])!="string"){
continue;
}
if(!this.Owner.IsClientEventName(clientEvent)){
if(_1ba[clientEvent]!=""){
var _1bb=_1ba[clientEvent];
if(_1bb.indexOf("(")!=-1){
this[clientEvent]=_1bb;
}else{
this[clientEvent]=eval(_1bb);
}
}else{
this[clientEvent]=null;
}
}
}
};
RadGridNamespace.RadGridTable.prototype.DisposeEvents=function(){
for(var _1bc in RadGridNamespace.RadGridTable.ClientEventNames){
this[_1bc]=null;
}
};
RadGridNamespace.RadGridTable.prototype.InitializeRows=function(rows){
if(this.ClientID.indexOf("_Header")!=-1||this.ClientID.indexOf("_Footer")!=-1){
return;
}
try{
var _1be=[];
for(var i=0;i<rows.length;i++){
if(!rows[i].Visible||rows[i].ClientRowIndex<0){
continue;
}
if(rows[i].ItemType=="THead"||rows[i].ItemType=="TFoot"||rows[i].ItemType=="NoRecordsItem"){
continue;
}
RadGridNamespace.FireEvent(this,"OnRowCreating");
rows[i]._constructor(this);
_1be[_1be.length]=rows[i];
RadGridNamespace.FireEvent(this,"OnRowCreated",[rows[i]]);
}
this.Rows=_1be;
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.DisposeRows=function(){
if(this.Rows!=null){
for(var i=0;i<this.Rows.length;i++){
var row=this.Rows[i];
row.Dispose();
}
this.Rows=null;
}
};
RadGridNamespace.RadGridTable.prototype.DisposeColumns=function(){
if(this.Columns!=null){
for(var i=0;i<this.Columns.length;i++){
var _1c3=this.Columns[i];
_1c3.Dispose();
}
this.Columns=null;
}
};
RadGridNamespace.RadGridTable.prototype.GetTableRows=function(_1c4,_1c5){
if(this.ClientID.indexOf("_Header")!=-1||this.ClientID.indexOf("_Footer")!=-1){
return;
}
try{
var _1c6=this;
var _1c7=new Array();
var j=0;
for(var i=0;i<_1c5.length;i++){
if((_1c5[i].ItemType=="THead")||(_1c5[i].ItemType=="TFoot")){
continue;
}
if((_1c5[i])&&(_1c5[i].Visible)){
RadGridNamespace.FireEvent(this,"OnRowCreating");
setTimeout(function(){
_1c7[_1c7.length]=_1c5[i]._constructor(_1c6);
},0);
RadGridNamespace.FireEvent(this,"OnRowCreated",[_1c7[j]]);
j++;
}
}
return _1c7;
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.GetTableHeaderRow=function(){
try{
if(this.Control.tHead){
for(var i=0;i<this.Control.tHead.rows.length;i++){
if(this.Control.tHead.rows[i]!=null){
if(this.Control.tHead.rows[i].cells[0]!=null){
if(this.Control.tHead.rows[i].cells[0].tagName!=null){
if(this.Control.tHead.rows[i].cells[0].tagName.toLowerCase()=="th"){
this.HeaderRow=this.Control.tHead.rows[i];
break;
}
}
}
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.GetTableColumns=function(_1cb,_1cc){
try{
this.GetTableHeaderRow();
var _1cd=new Array();
if(!this.HeaderRow){
return;
}
if(!this.HeaderRow.cells[0]){
return;
}
var j=0;
for(var i=0;i<_1cc.length;i++){
if(_1cc[i].Visible){
RadGridNamespace.FireEvent(this,"OnColumnCreating");
_1cd[_1cd.length]=new RadGridNamespace.RadGridTableColumn(_1cc[i]);
if(this.HeaderRow.cells[j]!=null){
_1cd[j]._constructor(this.HeaderRow.cells[j],this);
_1cd[j].RealIndex=i;
if(_1cc[i].ColumnType=="GridExpandColumn"){
this.ExpandCollapseColumns[this.ExpandCollapseColumns.length]=_1cd[j];
}
if(_1cc[i].ColumnType=="GridGroupSplitterColumn"){
this.GroupSplitterColumns[this.GroupSplitterColumns.length]=_1cd[j];
}
if(_1cc[i].ColumnType=="GridRowIndicatorColumn"){
if(this.ClientID.indexOf("_Header")!=-1){
this.Owner.ClientSettings.Scrolling.FrozenColumnsCount++;
}
}
RadGridNamespace.FireEvent(this,"OnColumnCreated",[_1cd[j]]);
}
j++;
}
}
return _1cd;
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.RemoveTableLayOut=function(){
this.masterTableLayOut=this.Owner.MasterTableView.Control.style.tableLayout;
this.detailTablesTableLayOut=new Array();
for(var i=0;i<this.Owner.DetailTablesCollection.length;i++){
this.detailTablesTableLayOut[this.detailTablesTableLayOut.length]=this.Owner.DetailTablesCollection[i].Control.style.tableLayout;
this.Owner.DetailTablesCollection[i].Control.style.tableLayout="";
}
};
RadGridNamespace.RadGridTable.prototype.RestoreTableLayOut=function(){
this.Owner.MasterTableView.Control.style.tableLayout=this.masterTableLayOut;
for(var i=0;i<this.Owner.DetailTablesCollection.length;i++){
this.Owner.DetailTablesCollection[i].Control.style.tableLayout=this.detailTablesTableLayOut[i];
}
};
RadGridNamespace.RadGridTable.prototype.SelectRow=function(row,_1d3){
try{
if(!this.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
var _1d4=this.Owner.GetRowObjectByRealRow(this,row);
if(_1d4!=null){
if(_1d4.ItemType=="Item"||_1d4.ItemType=="AlternatingItem"){
_1d4.SetSelected(_1d3);
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.DeselectRow=function(row){
try{
if(!this.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
var _1d6=this.Owner.GetRowObjectByRealRow(this,row);
if(_1d6!=null){
if(_1d6.ItemType=="Item"||_1d6.ItemType=="AlternatingItem"){
this.RemoveFromSelectedRows(_1d6);
_1d6.RemoveSelectedRowStyle();
_1d6.Selected=false;
_1d6.CheckClientSelectColumns();
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ResizeRow=function(_1d7,_1d8,_1d9){
try{
if(!this.Owner.ClientSettings.Resizing.AllowRowResize){
return;
}
if(!RadGridNamespace.FireEvent(this,"OnRowResizing",[_1d7,_1d8])){
return;
}
this.RemoveTableLayOut();
var _1da=this.Control.style.tableLayout;
this.Control.style.tableLayout="";
var _1db=this.Control.parentNode.parentNode.parentNode.parentNode;
var _1dc=this.Owner.GetTableObjectByID(_1db.id);
var _1dd;
if(_1dc!=null){
_1dd=_1dc.Control.style.tableLayout;
_1dc.Control.style.tableLayout="";
}
if(!_1d9){
if(this.Control){
if(this.Control.rows[_1d7]){
if(this.Control.rows[_1d7].cells[0]){
this.Control.rows[_1d7].cells[0].style.height=_1d8+"px";
this.Control.rows[_1d7].style.height=_1d8+"px";
}
}
}
}else{
if(this.Control){
if(this.Control.tBodies[0]){
if(this.Control.tBodies[0].rows[_1d7]){
if(this.Control.tBodies[0].rows[_1d7].cells[0]){
this.Control.tBodies[0].rows[_1d7].cells[0].style.height=_1d8+"px";
this.Control.tBodies[0].rows[_1d7].style.height=_1d8+"px";
}
}
}
}
}
this.Control.style.tableLayout=_1da;
if(_1dc!=null){
_1dc.Control.style.tableLayout=_1dd;
}
this.RestoreTableLayOut();
var _1de=this.Owner.GetRowObjectByRealRow(this,this.Control.rows[_1d7]);
this.Owner.SavePostData("ResizedRows",this.Control.id,_1de.RealIndex,_1d8+"px");
RadGridNamespace.FireEvent(this,"OnRowResized",[_1d7,_1d8]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ResizeColumn=function(_1df,_1e0){
if(isNaN(parseInt(_1df))){
var _1e1="Column index must be of type \"Number\"!";
alert(_1e1);
return;
}
if(isNaN(parseInt(_1e0))){
var _1e1="Column width must be of type \"Number\"!";
alert(_1e1);
return;
}
if(_1df<0){
var _1e1="Column index must be non-negative!";
alert(_1e1);
return;
}
if(_1e0<0){
var _1e1="Column width must be non-negative!";
alert(_1e1);
return;
}
if(_1df>(this.Columns.length-1)){
var _1e1="Column index must be less than columns count!";
alert(_1e1);
return;
}
if(!this.Owner.ClientSettings.Resizing.AllowColumnResize){
return;
}
if(!this.Columns){
return;
}
if(!this.Columns[_1df].Resizable){
return;
}
if(!RadGridNamespace.FireEvent(this,"OnColumnResizing",[_1df,_1e0])){
return;
}
try{
if(this==this.Owner.MasterTableView&&this.Owner.MasterTableViewHeader){
this.Owner.MasterTableViewHeader.ResizeColumn(_1df,_1e0);
}
var _1e2=this.Control.clientWidth;
var _1e3=this.Owner.Control.clientWidth;
if(this.HeaderRow){
var _1e4=this.HeaderRow.cells[_1df].scrollWidth-_1e0;
}
if(window.netscape||window.opera){
if(this.HeaderRow){
if(this.HeaderRow.cells[_1df]){
this.HeaderRow.cells[_1df].style.width=_1e0+"px";
}
}
if(this==this.Owner.MasterTableViewHeader){
var _1e5=this.Owner.MasterTableView.Control.tBodies[0].rows[this.Owner.ClientSettings.FirstDataRowClientRowIndex];
if(_1e5){
if(_1e5.cells[_1df]){
_1e5.cells[_1df].style.width=_1e0+"px";
}
}
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.Control){
if(this.Owner.MasterTableViewFooter.Control.tBodies[0].rows[0]&&this.Owner.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[_1df]){
if(_1e0>0){
this.Owner.MasterTableViewFooter.Control.tBodies[0].rows[0].cells[_1df].style.width=_1e0+"px";
}
}
}
}
}
if(this.ColGroup){
if(this.ColGroup.Cols[_1df]){
if(_1e0>0){
this.ColGroup.Cols[_1df].width=_1e0+"px";
}
}
}
if(this==this.Owner.MasterTableViewHeader){
if(this.Owner.MasterTableView.ColGroup){
if(this.Owner.MasterTableView.ColGroup.Cols[_1df]){
if(_1e0>0){
this.Owner.MasterTableView.ColGroup.Cols[_1df].width=_1e0+"px";
}
}
}
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.ColGroup){
if(this.Owner.MasterTableViewFooter.ColGroup.Cols[_1df]){
if(_1e0>0){
this.Owner.MasterTableViewFooter.ColGroup.Cols[_1df].width=_1e0+"px";
}
}
}
}
if(_1e0.toString().indexOf("px")!=-1){
_1e0=_1e0.replace("px","");
}
if(this==this.Owner.MasterTableView||this==this.Owner.MasterTableViewHeader){
if(_1e0.toString().indexOf("%")!=-1){
this.Owner.SavePostData("ResizedColumns",this.Owner.MasterTableView.ClientID,this.Columns[_1df].RealIndex,_1e0);
}else{
this.Owner.SavePostData("ResizedColumns",this.Owner.MasterTableView.ClientID,this.Columns[_1df].RealIndex,_1e0+"px");
}
}else{
if(_1e0.toString().indexOf("%")!=-1){
this.Owner.SavePostData("ResizedColumns",this.ClientID,this.Columns[_1df].RealIndex,_1e0);
}else{
this.Owner.SavePostData("ResizedColumns",this.ClientID,this.Columns[_1df].RealIndex,_1e0+"px");
}
}
if(this.Owner.MasterTableViewHeader){
this.Owner.ClientSettings.Resizing.ResizeGridOnColumnResize=true;
}
if(this.Owner.ClientSettings.Resizing.ResizeGridOnColumnResize){
if(this==this.Owner.MasterTableViewHeader){
for(var i=0;i<this.ColGroup.Cols.length;i++){
if(i!=_1df&&this.ColGroup.Cols[i].width==""){
this.ColGroup.Cols[i].width=this.HeaderRow.cells[i].scrollWidth+"px";
this.Owner.MasterTableView.ColGroup.Cols[i].width=this.ColGroup.Cols[i].width;
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.ColGroup){
this.Owner.MasterTableViewFooter.ColGroup.Cols[i].width=this.ColGroup.Cols[i].width;
}
}
}
this.Control.style.width=(this.Control.offsetWidth-_1e4)+"px";
this.Owner.MasterTableView.Control.style.width=this.Control.style.width;
if(this.Owner.MasterTableViewFooter&&this.Owner.MasterTableViewFooter.Control){
this.Owner.MasterTableViewFooter.Control.style.width=this.Control.style.width;
}
var _1e7=(this.Control.scrollWidth>this.Control.offsetWidth)?this.Control.scrollWidth:this.Control.offsetWidth;
var _1e8=this.Owner.GridDataDiv.offsetWidth;
this.Owner.SavePostData("ResizedControl",this.ClientID,_1e7+"px",_1e8+"px",this.Owner.Control.offsetHeight+"px");
}else{
if(window.netscape||window.opera){
this.Control.style.width=(this.Control.offsetWidth-_1e4)+"px";
this.Owner.Control.style.width=this.Control.style.width;
}
var _1e7=(this.Control.scrollWidth>this.Control.offsetWidth)?this.Control.scrollWidth:this.Control.offsetWidth;
this.Owner.SavePostData("ResizedControl",this.ClientID,_1e7+"px",this.Owner.Control.offsetWidth+"px",this.Owner.Control.offsetHeight+"px");
}
}else{
var _1e9=(this.Control.offsetWidth-_1e3)/this.ColGroup.Cols.length;
var _1ea="";
for(var i=_1df+1;i<this.ColGroup.Cols.length;i++){
var _1eb=0;
if(this.ColGroup.Cols[i].width!=""){
_1eb=parseInt(this.ColGroup.Cols[i].width)-_1e9;
}
if(this.HeaderRow){
_1eb=this.HeaderRow.cells[i].scrollWidth-_1e9;
}
this.ColGroup.Cols[i].width="";
if(this==this.Owner.MasterTableViewHeader){
this.Owner.MasterTableView.ColGroup.Cols[i].width="";
}
if(this.Owner.MasterTableViewFooter){
this.Owner.MasterTableViewFooter.ColGroup.Cols[i].width="";
}
}
if(_1e3>0){
this.Owner.Control.style.width=_1e3+"px";
}
this.Control.style.width=_1e2+"px";
if(this==this.Owner.MasterTableViewHeader){
this.Owner.MasterTableView.Control.style.width=this.Control.style.width;
}
if(this.Owner.MasterTableViewFooter){
this.Owner.MasterTableViewFooter.Control.style.width=this.Control.style.width;
}
}
if(this.Owner.GroupPanelObject&&this.Owner.GroupPanelObject.Items.length>0&&navigator.userAgent.toLowerCase().indexOf("msie")!=-1){
if(this.Owner.MasterTableView&&this.Owner.MasterTableViewHeader){
this.Owner.MasterTableView.Control.style.width=this.Owner.MasterTableViewHeader.Control.offsetWidth+"px";
}
}
RadGridNamespace.FireEvent(this,"OnColumnResized",[_1df,_1e0]);
if(window.netscape){
this.Control.style.cssText=this.Control.style.cssText;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ReorderColumns=function(_1ec,_1ed){
if(isNaN(parseInt(_1ec))){
var _1ee="First column index must be of type \"Number\"!";
alert(_1ee);
return;
}
if(isNaN(parseInt(_1ed))){
var _1ee="Second column index must be of type \"Number\"!";
alert(_1ee);
return;
}
if(_1ec<0){
var _1ee="First column index must be non-negative!";
alert(_1ee);
return;
}
if(_1ed<0){
var _1ee="Second column index must be non-negative!";
alert(_1ee);
return;
}
if(_1ec>(this.Columns.length-1)){
var _1ee="First column index must be less than columns count!";
alert(_1ee);
return;
}
if(_1ed>(this.Columns.length-1)){
var _1ee="Second column index must be less than columns count!";
alert(_1ee);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
if(!this.Columns){
return;
}
if(!this.Columns[_1ec].Reorderable){
return;
}
if(!this.Columns[_1ed].Reorderable){
return;
}
this.SwapColumns(_1ec,_1ed);
if((!this.Owner.ClientSettings.ReorderColumnsOnClient)&&(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder!="")){
if(this==this.Owner.MasterTableView){
eval(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder);
}
}
};
RadGridNamespace.RadGridTable.prototype.SwapColumns=function(_1ef,_1f0,_1f1){
if(isNaN(parseInt(_1ef))){
var _1f2="First column index must be of type \"Number\"!";
alert(_1f2);
return;
}
if(isNaN(parseInt(_1f0))){
var _1f2="Second column index must be of type \"Number\"!";
alert(_1f2);
return;
}
if(_1ef<0){
var _1f2="First column index must be non-negative!";
alert(_1f2);
return;
}
if(_1f0<0){
var _1f2="Second column index must be non-negative!";
alert(_1f2);
return;
}
if(_1ef>(this.Columns.length-1)){
var _1f2="First column index must be less than columns count!";
alert(_1f2);
return;
}
if(_1f0>(this.Columns.length-1)){
var _1f2="Second column index must be less than columns count!";
alert(_1f2);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
if(!this.Columns){
return;
}
if(!this.Columns[_1ef].Reorderable){
return;
}
if(!this.Columns[_1f0].Reorderable){
return;
}
try{
if(this==this.Owner.MasterTableView&&this.Owner.MasterTableViewHeader){
this.Owner.MasterTableViewHeader.SwapColumns(_1ef,_1f0,!this.Owner.ClientSettings.ReorderColumnsOnClient);
return;
}
if(typeof (_1f1)=="undefined"){
_1f1=true;
}
if(this.Owner.ClientSettings.ColumnsReorderMethod=="Reorder"){
if(_1f0>_1ef){
while(_1ef+1<_1f0){
this.SwapColumns(_1f0-1,_1f0,false);
_1f0--;
}
}else{
while(_1f0<_1ef-1){
this.SwapColumns(_1f0+1,_1f0,false);
_1f0++;
}
}
}
if(!RadGridNamespace.FireEvent(this,"OnColumnSwapping",[_1ef,_1f0])){
return;
}
var _1f3=this.Control;
var _1f4=this.Columns[_1ef];
var _1f5=this.Columns[_1f0];
this.Columns[_1ef]=_1f5;
this.Columns[_1f0]=_1f4;
var _1f6=this.ColGroup.Cols[_1ef].width;
if(_1f6==""&&this.HeaderRow){
_1f6=this.HeaderRow.cells[_1ef].offsetWidth;
}
var _1f7=this.ColGroup.Cols[_1f0].width;
if(_1f7==""&&this.HeaderRow){
_1f7=this.HeaderRow.cells[_1f0].offsetWidth;
}
var _1f8=this.Owner.ClientSettings.Resizing.AllowColumnResize;
var _1f9=(typeof (this.Columns[_1ef].Resizable)=="boolean")?this.Columns[_1ef].Resizable:false;
var _1fa=(typeof (this.Columns[_1f0].Resizable)=="boolean")?this.Columns[_1f0].Resizable:false;
this.Owner.ClientSettings.Resizing.AllowColumnResize=true;
this.Columns[_1ef].Resizable=true;
this.Columns[_1f0].Resizable=true;
this.ResizeColumn(_1ef,_1f7);
this.ResizeColumn(_1f0,_1f6);
this.Owner.ClientSettings.Resizing.AllowColumnResize=_1f8;
this.Columns[_1ef].Resizable=_1f9;
this.Columns[_1f0].Resizable=_1fa;
if(navigator.userAgent.indexOf("Safari")!=-1){
var _1fb=this.Columns[_1ef].Control;
var _1fc=this.Columns[_1f0].Control;
this.Columns[_1ef].Control=_1fc;
this.Columns[_1f0].Control=_1fb;
}
var _1fd=(this==this.Owner.MasterTableViewHeader)?this.Owner.MasterTableView.ClientID:this.ClientID;
this.Owner.SavePostData("ReorderedColumns",_1fd,this.Columns[_1ef].UniqueName,this.Columns[_1f0].UniqueName);
for(var i=0;i<_1f3.rows.length;i++){
if(_1f3.rows[i]!=null){
if((_1f3.rows[i].cells[_1ef]!=null)&&(_1f3.rows[i].cells[_1f0]!=null)){
if(_1f3.rows[i].cells[_1ef].innerHTML!=null){
var _1ff=_1f3.rows[i].cells[_1ef].innerHTML;
var _200=_1f3.rows[i].cells[_1f0].innerHTML;
_1f3.rows[i].cells[_1ef].innerHTML=_200;
_1f3.rows[i].cells[_1f0].innerHTML=_1ff;
}
}
}
}
if(this.Owner.MasterTableViewHeader==this){
var _1f3=this.Owner.MasterTableView.Control;
for(var i=0;i<_1f3.rows.length;i++){
if(_1f3.rows[i]!=null){
if((_1f3.rows[i].cells[_1ef]!=null)&&(_1f3.rows[i].cells[_1f0]!=null)){
if(_1f3.rows[i].cells[_1ef].innerHTML!=null){
var _1ff=_1f3.rows[i].cells[_1ef].innerHTML;
var _200=_1f3.rows[i].cells[_1f0].innerHTML;
_1f3.rows[i].cells[_1ef].innerHTML=_200;
_1f3.rows[i].cells[_1f0].innerHTML=_1ff;
}
}
}
}
}
if(_1f1&&(!this.Owner.ClientSettings.ReorderColumnsOnClient)&&(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder!="")){
eval(this.Owner.ClientSettings.PostBackReferences.PostBackColumnsReorder);
}
RadGridNamespace.FireEvent(this,"OnColumnSwapped",[_1ef,_1f0]);
this.Owner.InitializeFilterMenu(this);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.MoveColumnToLeft=function(_201){
if(isNaN(parseInt(_201))){
var _202="Column index must be of type \"Number\"!";
alert(_202);
return;
}
if(_201<0){
var _202="Column index must be non-negative!";
alert(_202);
return;
}
if(_201>(this.Columns.length-1)){
var _202="Column index must be less than columns count!";
alert(_202);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnMovingToLeft",[_201])){
return;
}
var _203=_201--;
this.SwapColumns(_201,_203);
RadGridNamespace.FireEvent(this,"OnColumnMovedToLeft",[_201]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.MoveColumnToRight=function(_204){
if(isNaN(parseInt(_204))){
var _205="Column index must be of type \"Number\"!";
alert(_205);
return;
}
if(_204<0){
var _205="Column index must be non-negative!";
alert(_205);
return;
}
if(_204>(this.Columns.length-1)){
var _205="Column index must be less than columns count!";
alert(_205);
return;
}
if(!this.Owner.ClientSettings.AllowColumnsReorder){
return;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnMovingToRight",[_204])){
return;
}
var _206=_204++;
this.SwapColumns(_204,_206);
RadGridNamespace.FireEvent(this,"OnColumnMovedToRight",[_204]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.CanShowHideColumn=function(_207){
if(!this.Owner.ClientSettings.AllowColumnHide){
return false;
}
if(isNaN(parseInt(_207))){
var _208="Column index must be of type \"Number\"!";
alert(_208);
return false;
}
if(_207<0){
var _208="Column index must be non-negative!";
alert(_208);
return false;
}
if(_207>(this.Columns.length-1)){
var _208="Column index must be less than columns count!";
alert(_208);
return false;
}
return true;
};
RadGridNamespace.RadGridTable.prototype.HideNotFrozenColumn=function(_209){
this.HideShowNotFrozenColumn(_209,false);
};
RadGridNamespace.RadGridTable.prototype.ShowNotFrozenColumn=function(_20a){
this.HideShowNotFrozenColumn(_20a,true);
};
RadGridNamespace.RadGridTable.prototype.HideShowNotFrozenColumn=function(_20b,_20c){
if(this.Owner.MasterTableViewHeader){
this.Owner.MasterTableViewHeader.Columns[_20b].FrozenDisplay=_20c;
if(!window.netscape&&navigator.userAgent.toLowerCase().indexOf("safari")==-1){
this.HideShowCol(this.Owner.MasterTableViewHeader,_20b,_20c);
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&navigator.userAgent.toLowerCase().indexOf("6.0")!=-1){
var _20d=this.Owner.MasterTableViewHeader.Control.getElementsByTagName("select");
if(_20d.length>0){
var _20e=this;
setTimeout(function(){
RadGridNamespace.HideShowCells(_20e.Owner.MasterTableViewHeader.Control,_20b,_20c,_20e.Owner.MasterTableViewHeader.ColGroup.Cols);
},0);
}
}
}else{
RadGridNamespace.HideShowCells(this.Owner.MasterTableViewHeader.Control,_20b,_20c,this.Owner.MasterTableViewHeader.ColGroup.Cols);
}
}
if(this.Owner.MasterTableView){
this.Owner.MasterTableView.Columns[_20b].FrozenDisplay=_20c;
if(!window.netscape&&navigator.userAgent.toLowerCase().indexOf("safari")==-1){
this.HideShowCol(this.Owner.MasterTableView,_20b,_20c);
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&navigator.userAgent.toLowerCase().indexOf("6.0")!=-1){
var _20d=this.Owner.MasterTableView.Control.getElementsByTagName("select");
if(_20d.length>0){
var _20e=this;
setTimeout(function(){
RadGridNamespace.HideShowCells(_20e.Owner.MasterTableView.Control,_20b,_20c,_20e.Owner.MasterTableView.ColGroup.Cols);
},0);
}
}
}else{
RadGridNamespace.HideShowCells(this.Owner.MasterTableView.Control,_20b,_20c,this.Owner.MasterTableView.ColGroup.Cols);
}
}
if(this.Owner.MasterTableViewFooter){
this.Owner.MasterTableViewFooter.Columns[_20b].FrozenDisplay=_20c;
if(!window.netscape&&navigator.userAgent.toLowerCase().indexOf("safari")==-1){
this.HideShowCol(this.Owner.MasterTableViewFooter,_20b,_20c);
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&navigator.userAgent.toLowerCase().indexOf("6.0")!=-1){
var _20d=this.Owner.MasterTableViewFooter.Control.getElementsByTagName("select");
if(_20d.length>0){
var _20e=this;
setTimeout(function(){
RadGridNamespace.HideShowCells(_20e.Owner.MasterTableViewFooter.Control,_20b,_20c,_20e.Owner.MasterTableViewFooter.ColGroup.Cols);
},0);
}
}
}else{
RadGridNamespace.HideShowCells(this.Owner.MasterTableViewFooter.Control,_20b,_20c,this.Owner.MasterTableViewFooter.ColGroup.Cols);
}
}
};
RadGridNamespace.RadGridTable.prototype.HideColumn=function(_20f){
if(!this.CanShowHideColumn(_20f)){
return false;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnHiding",[_20f])){
return;
}
this.HideShowColumn(_20f,false);
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("HidedColumns",this.ClientID,this.Columns[_20f].RealIndex);
}
RadGridNamespace.FireEvent(this,"OnColumnHidden",[_20f]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ShowColumn=function(_210){
if(!this.CanShowHideColumn(_210)){
return false;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnColumnShowing",[_210])){
return;
}
this.HideShowColumn(_210,true);
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("ShowedColumns",this.ClientID,this.Columns[_210].RealIndex);
}
RadGridNamespace.FireEvent(this,"OnColumnShowed",[_210]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.HideShowCol=function(_211,_212,_213){
if(_211&&_211.ColGroup&&_211.ColGroup.Cols&&_211.ColGroup.Cols[_212]){
var _214=(_211.ColGroup.Cols[_212].style.display=="")?true:false;
if(_214!=_213){
_211.ColGroup.Cols[_212].style.display=(_213)?"":"none";
}
}
};
RadGridNamespace.RadGridTable.prototype.HideShowColumn=function(_215,_216){
var _216=this.Columns[_215].Display=_216;
if(this!=this.Owner.MasterTableViewHeader&&this!=this.Owner.MasterTableView&&this!=this.Owner.MasterTableViewFooter){
if(window.netscape){
this.HideShowCol(this,_215,_216);
}
RadGridNamespace.HideShowCells(this.Control,_215,_216,this.ColGroup.Cols);
return;
}
if(this.Owner.MasterTableViewHeader){
if(window.netscape){
this.HideShowCol(this.Owner.MasterTableViewHeader,_215,_216);
}
RadGridNamespace.HideShowCells(this.Owner.MasterTableViewHeader.Control,_215,_216,this.Owner.MasterTableViewHeader.ColGroup.Cols);
}
if(this.Owner.MasterTableView){
if(window.netscape){
this.HideShowCol(this.Owner.MasterTableView,_215,_216);
}
RadGridNamespace.HideShowCells(this.Owner.MasterTableView.Control,_215,_216,this.Owner.MasterTableView.ColGroup.Cols);
}
if(this.Owner.MasterTableViewFooter){
if(window.netscape){
this.HideShowCol(this.Owner.MasterTableViewFooter,_215,_216);
}
RadGridNamespace.HideShowCells(this.Owner.MasterTableViewFooter.Control,_215,_216,this.Owner.MasterTableViewFooter.ColGroup.Cols);
}
};
RadGridNamespace.RadGridTable.prototype.CanShowHideRow=function(_217){
if(!this.Owner.ClientSettings.AllowRowHide){
return false;
}
if(isNaN(parseInt(_217))){
var _218="Row index must be of type \"Number\"!";
alert(_218);
return false;
}
if(_217<0){
var _218="Row index must be non-negative!";
alert(_218);
return false;
}
if(_217>(this.Rows.length-1)){
var _218="Row index must be less than rows count!";
alert(_218);
return false;
}
return true;
};
RadGridNamespace.RadGridTable.prototype.HideRow=function(_219){
if(!this.CanShowHideRow(_219)){
return false;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnRowHiding",[_219])){
return;
}
if(this.Rows){
if(this.Rows[_219]){
if(this.Rows[_219].Control){
this.Rows[_219].Control.style.display="none";
this.Rows[_219].Display=false;
}
}
}
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("HidedRows",this.ClientID,this.Rows[_219].RealIndex);
}
RadGridNamespace.FireEvent(this,"OnRowHidden",[_219]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ShowRow=function(_21a){
if(!this.CanShowHideRow(_21a)){
return false;
}
try{
if(!RadGridNamespace.FireEvent(this,"OnRowShowing",[_21a])){
return;
}
if(this.Rows){
if(this.Rows[_21a]){
if(this.Rows[_21a].Control){
if(this.Rows[_21a].ItemType!="NestedView"){
if(window.netscape){
this.Rows[_21a].Control.style.display="table-row";
}else{
this.Rows[_21a].Control.style.display="";
}
this.Rows[_21a].Display=true;
}
}
}
}
if(this!=this.Owner.MasterTableViewHeader){
this.Owner.SavePostData("ShowedRows",this.ClientID,this.Rows[_21a].RealIndex);
}
RadGridNamespace.FireEvent(this,"OnRowShowed",[_21a]);
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.OnError);
}
};
RadGridNamespace.RadGridTable.prototype.ExportToExcel=function(_21b){
try{
this.Owner.SavePostData("ExportToExcel",this.ClientID,_21b);
__doPostBack(this.Owner.UniqueID,"ExportToExcel");
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.ExportToWord=function(_21c){
try{
this.Owner.SavePostData("ExportToWord",this.ClientID,_21c);
__doPostBack(this.Owner.UniqueID,"ExportToWord");
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.ExportToCSV=function(_21d){
try{
this.Owner.SavePostData("ExportToCSV",this.ClientID,_21d);
__doPostBack(this.Owner.UniqueID,"ExportToCSV");
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.ExportToPdf=function(_21e){
try{
this.Owner.SavePostData("ExportToPdf",this.ClientID,_21e);
__doPostBack(this.Owner.UniqueID,"ExportToPdf");
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.AddToSelectedRows=function(_21f){
try{
this.SelectedRows[this.SelectedRows.length]=_21f;
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.IsInSelectedRows=function(_220){
try{
for(var i=0;i<this.SelectedRows.length;i++){
if(this.SelectedRows[i]!=_220){
return true;
}
}
return false;
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.ClearSelectedRows=function(){
var _222=this.SelectedRows;
for(var i=0;i<this.SelectedRows.length;i++){
if(!RadGridNamespace.FireEvent(this,"OnRowDeselecting",[this.SelectedRows[i]])){
continue;
}
this.SelectedRows[i].Selected=false;
this.SelectedRows[i].CheckClientSelectColumns();
this.SelectedRows[i].RemoveSelectedRowStyle();
var last=this.SelectedRows[i];
try{
this.SelectedRows.splice(i,1);
i--;
}
catch(ex){
}
RadGridNamespace.FireEvent(this,"OnRowDeselected",[last]);
}
this.SelectedRows=new Array();
};
RadGridNamespace.RadGridTable.prototype.RemoveRow=function(_225){
try{
var rows=new Array();
for(var i=0;i<this.Rows.length;i++){
var last=this.Rows[i];
if(this.Rows[i]!=_225){
rows[rows.length]=this.Rows[i];
}
}
this.Rows=rows;
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.RemoveFromSelectedRows=function(_229){
try{
var _22a=new Array();
for(var i=0;i<this.SelectedRows.length;i++){
var last=this.SelectedRows[i];
if(this.SelectedRows[i]!=_229){
_22a[_22a.length]=this.SelectedRows[i];
}else{
if(!this.Owner.AllowMultiRowSelection){
if(!RadGridNamespace.FireEvent(this,"OnRowDeselecting",[this.SelectedRows[i]])){
continue;
}
}
try{
this.SelectedRows.splice(i,1);
i--;
}
catch(ex){
}
_229.CheckClientSelectColumns();
setTimeout(function(){
RadGridNamespace.FireEvent(_229.Owner,"OnRowDeselected",[_229]);
},100);
}
}
this.SelectedRows=_22a;
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.GetSelectedRowsIndexes=function(){
try{
var _22d=new Array();
for(var i=0;i<this.SelectedRows.length;i++){
_22d[_22d.length]=this.SelectedRows[i].RealIndex;
}
return _22d.join(",");
}
catch(e){
throw e;
}
};
RadGridNamespace.RadGridTable.prototype.GetCellByColumnUniqueName=function(_22f,_230){
if(this.ClientID.indexOf("_Header")!=-1){
return;
}
if((!_22f)||(!_230)){
return;
}
if(!this.Columns){
return;
}
for(var i=0;i<this.Columns.length;i++){
if(this.Columns[i].UniqueName.toUpperCase()==_230.toUpperCase()){
return _22f.Control.cells[i];
}
}
return null;
};
RadGridNamespace.RadGridTableColumn=function(_232){
if((!_232)||typeof (_232)!="object"){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
for(var _233 in _232){
this[_233]=_232[_233];
}
this.Type="RadGridTableColumn";
this.ResizeTolerance=5;
this.CanResize=false;
};
RadGridNamespace.RadGridTableColumn.prototype._constructor=function(_234,_235){
this.Control=_234;
this.Owner=_235;
this.Index=_234.cellIndex;
if((window.opera&&typeof (_234.cellIndex)=="undefined")||(navigator.userAgent.indexOf("Safari")!=-1)){
var _236=this;
setTimeout(function(){
_236.Index=RadGridNamespace.GetRealCellIndex(_236.Owner,_236.Control);
},200);
}
this.AttachDomEvent(this.Control,"click","OnClick");
this.AttachDomEvent(this.Control,"dblclick","OnDblClick");
this.AttachDomEvent(this.Control,"mousemove","OnMouseMove");
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
this.AttachDomEvent(this.Control,"mouseup","OnMouseUp");
this.AttachDomEvent(this.Control,"mouseover","OnMouseOver");
this.AttachDomEvent(this.Control,"mouseout","OnMouseOut");
this.AttachDomEvent(this.Control,"contextmenu","OnContextMenu");
};
RadGridNamespace.RadGridTableColumn.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
if(this.ColumnResizer){
this.ColumnResizer.Dispose();
}
this.Control=null;
this.Owner=null;
this.Index=null;
};
RadGridNamespace.RadGridTableColumn.prototype.OnContextMenu=function(e){
try{
this.Index=RadGridNamespace.GetRealCellIndex(this.Owner,this.Control);
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnContextMenu",[this.Index,e])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnClick=function(e){
try{
this.Index=RadGridNamespace.GetRealCellIndex(this.Owner,this.Control);
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnClick",[this.Index])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnDblClick=function(e){
try{
this.Index=RadGridNamespace.GetRealCellIndex(this.Owner,this.Control);
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnDblClick",[this.Index])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseMove=function(e){
if(this.Owner.Owner.ClientSettings&&this.Owner.Owner.ClientSettings.Resizing.AllowColumnResize&&this.Resizable&&this.Control.tagName.toLowerCase()=="th"){
var _23b=RadGridNamespace.GetEventPosX(e);
var _23c=RadGridNamespace.FindPosX(this.Control);
var endX=_23c+this.Control.offsetWidth;
var _23e=RadGridNamespace.GetCurrentElement(e);
if(!_23e.initalCursorStyle){
_23e.initalCursorStyle=_23e.style.cursor;
}
if(this.Owner.Owner.GridDataDiv&&!this.Owner.Owner.GridHeaderDiv&&!window.netscape){
var _23f=0;
if(document.body.currentStyle&&document.body.currentStyle.margin&&document.body.currentStyle.margin.indexOf("px")!=-1&&!window.opera){
_23f=parseInt(document.body.currentStyle.marginLeft);
}
this.ResizeTolerance=10;
}
if((_23b>=endX-this.ResizeTolerance)&&(_23b<=endX+this.ResizeTolerance)&&!this.Owner.Owner.MoveHeaderDiv){
this.Control.style.cursor="e-resize";
this.Control.title=this.Owner.Owner.ClientSettings.ClientMessages.DragToResize;
this.CanResize=true;
_23e.style.cursor="e-resize";
this.Owner.Owner.IsResize=true;
}else{
this.Control.style.cursor="";
this.Control.title="";
this.CanResize=false;
_23e.style.cursor=_23e.initalCursorStyle;
this.Owner.Owner.IsResize=false;
}
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseDown=function(e){
this.AttachDomEvent(document,"mouseup","OnMouseUp");
if(this.CanResize){
if(((window.netscape||window.opera||navigator.userAgent.indexOf("Safari")!=-1)&&(e.button==0))||(e.button==1)){
var _241=RadGridNamespace.GetEventPosX(e);
var _242=RadGridNamespace.FindPosX(this.Control);
var endX=_242+this.Control.offsetWidth;
if((_241>=endX-this.ResizeTolerance)&&(_241<=endX+this.ResizeTolerance)){
this.ColumnResizer=new RadGridNamespace.RadGridColumnResizer(this,this.Owner.Owner.ClientSettings.Resizing.EnableRealTimeResize);
this.ColumnResizer.Position(e);
}
}
RadGridNamespace.ClearDocumentEvents();
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseUp=function(e){
this.DetachDomEvent(document,"mouseup","OnMouseUp");
RadGridNamespace.RestoreDocumentEvents();
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseOver=function(e){
this.Index=RadGridNamespace.GetRealCellIndex(this.Owner,this.Control);
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnMouseOver",[this.Index])){
return;
}
if(this.Owner.Owner.Skin!=""&&this.Owner.Owner.Skin!="None"){
RadGridNamespace.addClassName(this.Control,"GridHeaderOver_"+this.Owner.Owner.Skin);
}
};
RadGridNamespace.RadGridTableColumn.prototype.OnMouseOut=function(e){
this.Index=RadGridNamespace.GetRealCellIndex(this.Owner,this.Control);
if(!RadGridNamespace.FireEvent(this.Owner,"OnColumnMouseOut",[this.Index])){
return;
}
if(this.Owner.Owner.Skin!=""&&this.Owner.Owner.Skin!="None"){
RadGridNamespace.removeClassName(this.Control,"GridHeaderOver_"+this.Owner.Owner.Skin);
}
};
RadGridNamespace.RadGridColumnResizer=function(_247,_248){
if(!_247){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
this.Column=_247;
this.IsRealTimeResize=_248;
this.CurrentWidth=null;
this.LeftResizer=document.createElement("span");
this.LeftResizer.style.backgroundColor="navy";
this.LeftResizer.style.width="1"+"px";
this.LeftResizer.style.position="absolute";
this.LeftResizer.style.cursor="e-resize";
this.RightResizer=document.createElement("span");
this.RightResizer.style.backgroundColor="navy";
this.RightResizer.style.width="1"+"px";
this.RightResizer.style.position="absolute";
this.RightResizer.style.cursor="e-resize";
this.ResizerToolTip=document.createElement("span");
this.ResizerToolTip.style.backgroundColor="#F5F5DC";
this.ResizerToolTip.style.border="1px solid";
this.ResizerToolTip.style.position="absolute";
this.ResizerToolTip.style.font="icon";
this.ResizerToolTip.style.padding="2";
this.ResizerToolTip.innerHTML="Width: <b>"+this.Column.Control.offsetWidth+"</b> <em>pixels</em>";
document.body.appendChild(this.LeftResizer);
document.body.appendChild(this.RightResizer);
document.body.appendChild(this.ResizerToolTip);
this.CanDestroy=true;
this.AttachDomEvent(document,"mouseup","OnMouseUp");
this.AttachDomEvent(this.Column.Owner.Owner.Control,"mousemove","OnMouseMove");
};
RadGridNamespace.RadGridColumnResizer.prototype.OnMouseUp=function(e){
this.Destroy(e);
};
RadGridNamespace.RadGridColumnResizer.prototype.OnMouseMove=function(e){
this.Move(e);
};
RadGridNamespace.RadGridColumnResizer.prototype.Position=function(e){
this.LeftResizer.style.top=RadGridNamespace.FindPosY(this.Column.Control)-RadGridNamespace.FindScrollPosY(this.Column.Control)+document.documentElement.scrollTop+document.body.scrollTop+"px";
this.LeftResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)-RadGridNamespace.FindScrollPosX(this.Column.Control)+document.documentElement.scrollLeft+document.body.scrollLeft+"px";
this.RightResizer.style.top=this.LeftResizer.style.top;
this.RightResizer.style.left=parseInt(this.LeftResizer.style.left)+this.Column.Control.offsetWidth+"px";
this.ResizerToolTip.style.top=parseInt(this.RightResizer.style.top)-20+"px";
this.ResizerToolTip.style.left=parseInt(this.RightResizer.style.left)-5+"px";
if(parseInt(this.LeftResizer.style.left)<RadGridNamespace.FindPosX(this.Column.Owner.Control)){
this.LeftResizer.style.display="none";
}
if(!this.Column.Owner.Owner.ClientSettings.Scrolling.AllowScroll){
this.LeftResizer.style.height=this.Column.Owner.Control.tBodies[0].offsetHeight+this.Column.Owner.Control.tHead.offsetHeight+"px";
}else{
if(this.Column.Owner.Owner.ClientSettings.Scrolling.UseStaticHeaders){
this.LeftResizer.style.height=this.Column.Owner.Owner.GridDataDiv.clientHeight+this.Column.Owner.Control.tHead.offsetHeight+"px";
}else{
this.LeftResizer.style.height=this.Column.Owner.Owner.GridDataDiv.clientHeight+"px";
}
}
this.RightResizer.style.height=this.LeftResizer.style.height;
if(!window.netscape){
this.Move(e);
}
};
RadGridNamespace.RadGridColumnResizer.prototype.Destroy=function(e){
if(this.CanDestroy){
this.DetachDomEvent(document,"mouseup","OnMouseUp");
this.DetachDomEvent(this.Column.Owner.Owner.Control,"mousemove","OnMouseMove");
if(this.CurrentWidth!=null){
if(this.CurrentWidth>0){
this.Column.Owner.ResizeColumn(this.Column.Control.cellIndex,this.CurrentWidth);
this.CurrentWidth=null;
}
}
document.body.removeChild(this.LeftResizer);
document.body.removeChild(this.RightResizer);
document.body.removeChild(this.ResizerToolTip);
this.CanDestroy=false;
}
};
RadGridNamespace.RadGridColumnResizer.prototype.Dispose=function(){
try{
this.Destroy();
}
catch(error){
}
this.DisposeDomEventHandlers();
this.MouseUpHandler=null;
this.MouseMoveHandler=null;
this.LeftResizer=null;
this.RightResizer=null;
this.ResizerToolTip=null;
};
RadGridNamespace.RadGridColumnResizer.prototype.Move=function(e){
this.LeftResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)-RadGridNamespace.FindScrollPosX(this.Column.Control)+document.documentElement.scrollLeft+document.body.scrollLeft+"px";
this.RightResizer.style.left=parseInt(this.LeftResizer.style.left)+(RadGridNamespace.GetEventPosX(e)-RadGridNamespace.FindPosX(this.Column.Control))+"px";
this.ResizerToolTip.style.left=parseInt(this.RightResizer.style.left)-5+"px";
var _24e=parseInt(this.RightResizer.style.left)-parseInt(this.LeftResizer.style.left);
var _24f=this.Column.Control.scrollWidth-_24e;
this.ResizerToolTip.innerHTML="Width: <b>"+_24e+"</b> <em>pixels</em>";
if(!RadGridNamespace.FireEvent(this.Column.Owner,"OnColumnResizing",[this.Column.Index,_24e])){
return;
}
if(_24e<=0){
this.RightResizer.style.left=this.RightResizer.style.left;
this.Destroy(e);
return;
}
this.CurrentWidth=_24e;
if(this.IsRealTimeResize){
var _250=(navigator.userAgent.indexOf("Safari")!=-1)?RadGridNamespace.GetRealCellIndex(this.Column.Owner,this.Column.Control):this.Column.Control.cellIndex;
this.Column.Owner.ResizeColumn(_250,_24e);
}else{
this.CurrentWidth=_24e;
return;
}
if(RadGridNamespace.FindPosX(this.LeftResizer)!=RadGridNamespace.FindPosX(this.Column.Control)){
this.LeftResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)+"px";
}
if(RadGridNamespace.FindPosX(this.RightResizer)!=(RadGridNamespace.FindPosX(this.Column.Control)+this.Column.Control.offsetWidth)){
this.RightResizer.style.left=RadGridNamespace.FindPosX(this.Column.Control)+this.Column.Control.offsetWidth+"px";
}
if(RadGridNamespace.FindPosY(this.LeftResizer)!=RadGridNamespace.FindPosY(this.Column.Control)){
this.LeftResizer.style.top=RadGridNamespace.FindPosY(this.Column.Control)+"px";
this.RightResizer.style.top=RadGridNamespace.FindPosY(this.Column.Control)+"px";
}
if(this.Column.Owner.Owner.GridDataDiv){
this.LeftResizer.style.left=parseInt(this.LeftResizer.style.left.replace("px",""))-this.Column.Owner.Owner.GridDataDiv.scrollLeft+"px";
this.RightResizer.style.left=parseInt(this.LeftResizer.style.left.replace("px",""))+this.Column.Control.offsetWidth+"px";
this.ResizerToolTip.style.left=parseInt(this.RightResizer.style.left)-5+"px";
}
if(!this.Column.Owner.Owner.ClientSettings.Scrolling.AllowScroll){
this.LeftResizer.style.height=this.Column.Owner.Control.tBodies[0].offsetHeight+this.Column.Owner.Control.tHead.offsetHeight+"px";
}else{
if(this.Column.Owner.Owner.ClientSettings.Scrolling.UseStaticHeaders){
this.LeftResizer.style.height=this.Column.Owner.Owner.GridDataDiv.clientHeight+this.Column.Owner.Control.tHead.offsetHeight+"px";
}else{
this.LeftResizer.style.height=this.Column.Owner.Owner.GridDataDiv.clientHeight+"px";
}
}
this.RightResizer.style.height=this.LeftResizer.style.height;
};
RadGridNamespace.RadGridTableRow=function(_251){
if((!_251)||typeof (_251)!="object"){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
for(var _252 in _251){
this[_252]=_251[_252];
}
this.Type="RadGridTableRow";
var _253=document.getElementById(this.OwnerID);
this.Control=_253.tBodies[0].rows[this.ClientRowIndex];
if(!this.Control){
return;
}
this.Index=this.Control.sectionRowIndex;
this.RealIndex=this.RowIndex;
};
RadGridNamespace.RadGridTableRow.prototype._constructor=function(_254){
this.Owner=_254;
this.CreateStyles();
if(this.Selected){
this.LoadSelected();
}
if(this.ItemType!="EditFormItem"){
this.CheckClientSelectColumns();
}
if(this.Owner.HierarchyLoadMode=="Client"){
if(this.Owner.Owner.ClientSettings.AllowExpandCollapse){
for(var i=0;i<this.Owner.ExpandCollapseColumns.length;i++){
var _256=this.Owner.ExpandCollapseColumns[i].Control.cellIndex;
var _257=this.Control.cells[_256];
var html=this.Control.innerHTML;
if(!_257){
continue;
}
var _259;
for(var j=0;j<_257.childNodes.length;j++){
if(!_257.childNodes[j].tagName){
continue;
}
var _25b;
if(this.Owner.ExpandCollapseColumns[i].ButtonType=="ImageButton"){
_25b="img";
}else{
if(this.Owner.ExpandCollapseColumns[i].ButtonType=="LinkButton"){
_25b="a";
}else{
if(this.Owner.ExpandCollapseColumns[i].ButtonType=="PushButton"){
_25b="button";
}
}
}
if(_257.childNodes[j].tagName.toLowerCase()==_25b){
_259=_257.childNodes[j];
break;
}
}
if(_259){
var _25c=this;
var _25d=function(){
_25c.OnHierarchyExpandButtonClick(this);
};
_259.onclick=_25d;
_259.ondblclick=null;
_25d=null;
}
_259=null;
}
}
}
if(this.Owner.GroupLoadMode=="Client"){
if(this.Owner.Owner.ClientSettings.AllowGroupExpandCollapse){
for(var i=0;i<this.Owner.GroupSplitterColumns.length;i++){
var _256=this.Owner.GroupSplitterColumns[i].Control.cellIndex;
var html=this.Control.innerHTML;
var _257=this.Control.cells[_256];
if(!_257){
continue;
}
var _259;
for(var j=0;j<_257.childNodes.length;j++){
if(!_257.childNodes[j].tagName){
continue;
}
if(_257.childNodes[j].tagName.toLowerCase()=="img"){
_259=_257.childNodes[j];
break;
}
}
if(_259){
var _25c=this;
var _25d=function(){
_25c.OnGroupExpandButtonClick(this);
};
_259.onclick=_25d;
_259.ondblclick=null;
_25d=null;
}
_259=null;
}
}
}
this.AttachDomEvent(this.Control,"click","OnClick");
this.AttachDomEvent(this.Control,"dblclick","OnDblClick");
this.AttachDomEvent(document,"mousedown","OnMouseDown");
this.AttachDomEvent(document,"mouseup","OnMouseUp");
this.AttachDomEvent(document,"mousemove","OnMouseMove");
this.AttachDomEvent(this.Control,"mouseover","OnMouseOver");
this.AttachDomEvent(this.Control,"mouseout","OnMouseOut");
this.AttachDomEvent(this.Control,"contextmenu","OnContextMenu");
if(this.Owner.Owner.ClientSettings.ActiveRowData&&this.Owner.Owner.ClientSettings.ActiveRowData!=""){
var data=this.Owner.Owner.ClientSettings.ActiveRowData.split(";")[0].split(",");
if(data[0]==this.Owner.ClientID&&data[1]==this.RealIndex){
this.Owner.Owner.ActiveRow=this;
}
}
};
RadGridNamespace.GroupRowExpander=function(_25f){
this.startRow=_25f;
};
RadGridNamespace.GroupRowExpander.prototype.NotFinished=function(_260){
var _261=(this.currentGridRow!=null);
if(!_261){
return false;
}
var _262=(this.currentGridRow.GroupIndex=="");
var _263=(this.currentGridRow.GroupIndex==_260.GroupIndex);
var _264=(this.currentGridRow.GroupIndex.indexOf(_260.GroupIndex+"_")==0);
return (_262||_263||_264);
};
RadGridNamespace.GroupRowExpander.prototype.ToggleExpandCollapse=function(_265){
var _266=this.startRow;
var _267=_266.Owner;
var _268=_265.parentNode.parentNode.sectionRowIndex;
var _269=_267.Rows[_268];
if(_269.Expanded){
if(!RadGridNamespace.FireEvent(_269.Owner,"OnGroupCollapsing",[_269])){
return;
}
}else{
if(!RadGridNamespace.FireEvent(_269.Owner,"OnGroupExpanding",[_269])){
return;
}
}
var _26a=_267.Control.rows[_268+1];
if(!_26a){
return;
}
this.currentRowIndex=_26a.rowIndex;
this.lastGroupIndex=null;
while(true){
this.currentGridRow=_267.Rows[this.currentRowIndex];
var _26b=this.NotFinished(_269);
if(!_26b){
break;
}
var _26c=(this.lastGroupIndex!=null)&&(this.currentGridRow.GroupIndex.indexOf(this.lastGroupIndex)!=-1);
var _26d=(this.currentGridRow.ItemType!="GroupHeader")&&(!this.currentGridRow.IsVisible());
var _26e=_26c&&_26d;
if(this.currentGridRow.ItemType=="GroupHeader"&&!this.currentGridRow.Expanded){
if(this.currentGridRow.IsVisible()){
this.currentGridRow.Hide();
_265.src=_267.GroupSplitterColumns[0].ExpandImageUrl;
_265.title=_267.Owner.GroupingSettings.ExpandTooltip;
if(_267.Rows[this.currentRowIndex+1]==null||_267.Rows[this.currentRowIndex+1].ItemType=="GroupHeader"){
this.currentGridRow.Expanded=false;
}
}else{
_265.src=_267.GroupSplitterColumns[0].CollapseImageUrl;
_265.title=_267.Owner.GroupingSettings.CollapseTooltip;
this.currentGridRow.Show();
if(_267.Rows[this.currentRowIndex+1]==null||_267.Rows[this.currentRowIndex+1].ItemType=="GroupHeader"){
this.currentGridRow.Expanded=true;
}
}
this.lastGroupIndex=this.currentGridRow.GroupIndex;
}else{
if(!_26e){
if(this.currentGridRow.ItemType=="NestedView"){
if(this.currentGridRow.Expanded){
if(this.currentGridRow.IsVisible()){
this.currentGridRow.Hide();
}else{
this.currentGridRow.Show();
}
}
}else{
if(this.currentGridRow.IsVisible()){
this.currentGridRow.Hide();
_265.src=_267.GroupSplitterColumns[0].ExpandImageUrl;
_265.title=_267.Owner.GroupingSettings.ExpandTooltip;
_269.Expanded=false;
}else{
_265.src=_267.GroupSplitterColumns[0].CollapseImageUrl;
_265.title=_267.Owner.GroupingSettings.CollapseTooltip;
this.currentGridRow.Show();
_269.Expanded=true;
}
}
}
}
this.currentRowIndex++;
}
if(_269.Expanded!=null){
if(_269.Expanded){
_267.Owner.SavePostData("ExpandedGroupRows",_267.ClientID,_269.RealIndex);
_266.title=_267.Owner.GroupingSettings.CollapseTooltip;
}else{
_267.Owner.SavePostData("CollapsedGroupRows",_267.ClientID,_269.RealIndex);
_266.title=_267.Owner.GroupingSettings.ExpandTooltip;
}
}
if(_269.Expanded){
if(!RadGridNamespace.FireEvent(_269.Owner,"OnGroupExpanded",[_269])){
return;
}
}else{
if(!RadGridNamespace.FireEvent(_269.Owner,"OnGroupCollapsed",[_269])){
return;
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnGroupExpandButtonClick=function(_26f){
var _270=new RadGridNamespace.GroupRowExpander(this);
_270.ToggleExpandCollapse(_26f);
};
RadGridNamespace.RadGridTableRow.prototype.OnHierarchyExpandButtonClick=function(_271){
var _272=this.Owner.Control.rows[_271.parentNode.parentNode.rowIndex+1];
var _273=this.Owner.Rows[_271.parentNode.parentNode.sectionRowIndex];
if(!_272){
return;
}
if(this.TableRowIsVisible(_272)){
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyCollapsing",[this])){
return;
}
this.HideTableRow(_272);
_273.Expanded=false;
if(this.Owner.ExpandCollapseColumns[0].ButtonType=="ImageButton"){
_271.src=this.Owner.ExpandCollapseColumns[0].ExpandImageUrl;
}else{
_271.innerHTML="+";
}
_271.title=this.Owner.Owner.HierarchySettings.ExpandTooltip;
this.Owner.Owner.SavePostData("CollapsedRows",this.Owner.ClientID,this.RealIndex);
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyCollapsed",[this])){
return;
}
}else{
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyExpanding",[this])){
return;
}
if(this.Owner.ExpandCollapseColumns[0].ButtonType=="ImageButton"){
_271.src=this.Owner.ExpandCollapseColumns[0].CollapseImageUrl;
}else{
_271.innerHTML="-";
}
_271.title=this.Owner.Owner.HierarchySettings.CollapseTooltip;
this.ShowTableRow(_272);
_273.Expanded=true;
this.Owner.Owner.SavePostData("ExpandedRows",this.Owner.ClientID,this.RealIndex);
if(!RadGridNamespace.FireEvent(this.Owner,"OnHierarchyExpanded",[this])){
return;
}
}
};
RadGridNamespace.RadGridTableRow.prototype.TableRowIsVisible=function(_274){
return _274.style.display!="none";
};
RadGridNamespace.RadGridTableRow.prototype.IsVisible=function(){
return this.TableRowIsVisible(this.Control);
};
RadGridNamespace.RadGridTableRow.prototype.HideTableRow=function(_275){
if(this.TableRowIsVisible(_275)){
_275.style.display="none";
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&navigator.userAgent.toLowerCase().indexOf("6.0")!=-1){
var _276=_275.getElementsByTagName("select");
for(var i=0;i<_276.length;i++){
_276[i].style.display="none";
}
}
}
};
RadGridNamespace.RadGridTableRow.prototype.Hide=function(){
this.HideTableRow(this.Control);
};
RadGridNamespace.RadGridTableRow.prototype.ShowTableRow=function(_278){
if(window.netscape||window.opera){
_278.style.display="table-row";
}else{
_278.style.display="block";
if(navigator.userAgent.toLowerCase().indexOf("msie")!=-1&&navigator.userAgent.toLowerCase().indexOf("6.0")!=-1){
var _279=_278.getElementsByTagName("select");
for(var i=0;i<_279.length;i++){
_279[i].style.display="";
}
}
}
};
RadGridNamespace.RadGridTableRow.prototype.Show=function(){
this.ShowTableRow(this.Control);
};
RadGridNamespace.RadGridTableRow.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
this.Control=null;
this.Owner=null;
};
RadGridNamespace.RadGridTableRow.prototype.CreateStyles=function(){
if(!this.Owner.Owner.ClientSettings.ApplyStylesOnClient){
return;
}
switch(this.ItemType){
case "GroupHeader":
break;
case "EditFormItem":
this.Control.className+=" "+this.Owner.RenderEditItemStyleClass;
this.Control.style.cssText+=" "+this.Owner.RenderEditItemStyle;
break;
default:
var _27b=eval("this.Owner.Render"+this.ItemType+"StyleClass");
if(typeof (_27b)!="undefined"){
this.Control.className+=" "+_27b;
}
var _27c=eval("this.Owner.Render"+this.ItemType+"Style");
if(typeof (_27c)!="undefined"){
this.Control.style.cssText+=" "+_27c;
}
break;
}
if(!this.Display){
if(this.Control.style.cssText!=""){
if(this.Control.style.cssText.lastIndexOf(";")==this.Control.style.cssText.length-1){
this.Control.style.cssText+="display:none;";
}else{
this.Control.style.cssText+=";display:none;";
}
}else{
this.Control.style.cssText+="display:none;";
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnContextMenu=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowContextMenu",[this.Index,e])){
return;
}
if(this.Owner.Owner.ClientSettings.ClientEvents.OnRowContextMenu!=""){
if(e.preventDefault){
e.preventDefault();
}else{
e.returnValue=false;
return false;
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableRow.prototype.OnClick=function(e){
try{
if(this.Owner.Owner.RowResizer){
return;
}
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowClick",[this.Control.sectionRowIndex,e])){
return;
}
if(this.ItemType=="EditFormItem"){
return;
}
if(e.shiftKey&&this.Owner.SelectedRows[0]){
if(this.Owner.SelectedRows[0].Control.rowIndex>this.Control.rowIndex){
for(var i=this.Control.rowIndex;i<this.Owner.SelectedRows[0].Control.rowIndex+1;i++){
var _280=this.Owner.Owner.GetRowObjectByRealRow(this.Owner,this.Owner.Control.rows[i]);
if(_280){
if(!_280.Selected){
this.Owner.SelectRow(this.Owner.Control.rows[i],false);
}
}
}
}
if(this.Owner.SelectedRows[0].Control.rowIndex<this.Control.rowIndex){
for(var i=this.Owner.SelectedRows[0].Control.rowIndex;i<this.Control.rowIndex+1;i++){
var _280=this.Owner.Owner.GetRowObjectByRealRow(this.Owner,this.Owner.Control.rows[i]);
if(_280){
if(!_280.Selected){
this.Owner.SelectRow(this.Owner.Control.rows[i],false);
}
}
}
}
}
if(!e.shiftKey){
this.HandleRowSelection(e);
}
var _281=RadGridNamespace.GetCurrentElement(e);
if(!_281){
return;
}
if(!_281.tagName){
return;
}
if(_281.tagName.toLowerCase()=="input"||_281.tagName.toLowerCase()=="select"||_281.tagName.toLowerCase()=="option"||_281.tagName.toLowerCase()=="button"||_281.tagName.toLowerCase()=="a"||_281.tagName.toLowerCase()=="textarea"){
return;
}
if(this.ItemType=="Item"||this.ItemType=="AlternatingItem"){
if(this.Owner.Owner.ClientSettings.EnablePostBackOnRowClick){
var _282=this.Owner.Owner.ClientSettings.PostBackFunction;
_282=_282.replace("{0}",this.Owner.Owner.UniqueID).replace("{1}","RowClick;"+this.ItemIndexHierarchical);
var form=document.getElementById(this.Owner.Owner.FormID);
if(form!=null&&form["__EVENTTARGET"]!=null&&form["__EVENTTARGET"].value==this.Owner.Owner.UniqueID){
form["__EVENTTARGET"].value="";
}
if(form!=null&&form["__EVENTTARGET"]!=null&&form["__EVENTTARGET"].value==""){
eval(_282);
}
}
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableRow.prototype.HandleActiveRow=function(e){
var _285=RadGridNamespace.GetCurrentElement(e);
if(_285!=null&&_285.tagName&&(_285.tagName.toLowerCase()=="input"||_285.tagName.toLowerCase()=="textarea")){
return;
}
var _286={13:"",40:"",39:"",38:"",37:"",32:""};
if(this.Owner.Owner.ActiveRow!=null){
if(!RadGridNamespace.FireEvent(this.Owner,"OnActiveRowChanging",[this.Owner.Owner.ActiveRow])){
return;
}
if(e.keyCode==13){
this.Owner.Owner.SavePostData("EditRow",this.Owner.ClientID,this.Owner.Owner.ActiveRow.RealIndex);
eval(this.Owner.Owner.ClientSettings.PostBackReferences.PostBackEditRow);
}
if(e.keyCode==40){
var _287=this.Owner.Rows[this.Owner.Owner.ActiveRow.Control.sectionRowIndex+1];
if(_287!=null){
this.Owner.Owner.SetActiveRow(_287);
this.ScrollIntoView(_287);
}
}
if(e.keyCode==39){
return;
var _287=this.Owner.Owner.GetNextHierarchicalRow(_288,this.Owner.Owner.ActiveRow.Control.sectionRowIndex);
if(_287!=null){
_288=_287.parentNode.parentNode;
this.Owner.Owner.SetActiveRow(_288,_287.sectionRowIndex);
this.ScrollIntoView(_287);
}
}
if(e.keyCode==38){
var _289=this.Owner.Rows[this.Owner.Owner.ActiveRow.Control.sectionRowIndex-1];
if(_289!=null){
this.Owner.Owner.SetActiveRow(_289);
this.ScrollIntoView(_289);
}
}
if(e.keyCode==37){
return;
var _289=this.Owner.Owner.GetPreviousHierarchicalRow(_288,this.Owner.Owner.ActiveRow.Control.sectionRowIndex);
if(_289!=null){
var _288=_289.parentNode.parentNode;
this.Owner.Owner.SetActiveRow(_288,_289.sectionRowIndex);
this.ScrollIntoView(_289);
}
}
if(e.keyCode==32){
if(this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect){
this.Owner.Owner.ActiveRow.Owner.SelectRow(this.Owner.Owner.ActiveRow.Control,!this.Owner.Owner.AllowMultiRowSelection);
}
}
}
RadGridNamespace.FireEvent(this.Owner,"OnActiveRowChanged",[this.Owner.Owner.ActiveRow]);
if(_286[e.keyCode]!=null){
if(window.netscape){
e.preventDefault();
return false;
}else{
e.returnValue=false;
}
}
};
RadGridNamespace.RadGridTableRow.prototype.ScrollIntoView=function(row){
if(row.Control&&row.Control.focus){
row.Control.scrollIntoView(false);
try{
row.Control.focus();
}
catch(e){
}
}
};
RadGridNamespace.RadGridTableRow.prototype.HandleExpandCollapse=function(){
};
RadGridNamespace.RadGridTableRow.prototype.HandleGroupExpandCollapse=function(){
};
RadGridNamespace.RadGridTableRow.prototype.HandleRowSelection=function(e){
var _28c=RadGridNamespace.GetCurrentElement(e);
if(_28c.onclick){
return;
}
if(_28c.tagName.toLowerCase()=="input"||_28c.tagName.toLowerCase()=="select"||_28c.tagName.toLowerCase()=="option"||_28c.tagName.toLowerCase()=="button"||_28c.tagName.toLowerCase()=="a"||_28c.tagName.toLowerCase()=="textarea"||_28c.tagName.toLowerCase()=="img"){
return;
}
this.SetSelected(!e.ctrlKey,e);
};
RadGridNamespace.RadGridTableRow.prototype.CheckClientSelectColumns=function(){
if(!this.Owner.Columns){
return;
}
for(var i=0;i<this.Owner.Columns.length;i++){
if(this.Owner.Columns[i].ColumnType=="GridClientSelectColumn"){
var cell=this.Owner.GetCellByColumnUniqueName(this,this.Owner.Columns[i].UniqueName);
if(cell!=null){
var _28f=cell.getElementsByTagName("input")[0];
if(_28f!=null){
_28f.checked=this.Selected;
}
}
}
}
};
RadGridNamespace.RadGridTableRow.prototype.SetSelected=function(_290,e){
if(!this.Selected){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowSelecting",[this,e])){
return;
}
}
if((this.ItemType=="Item")||(this.ItemType=="AlternatingItem")){
if(_290){
this.SingleSelect();
}else{
this.MultiSelect();
}
}
this.CheckClientSelectColumns();
if(this.Selected){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowSelected",[this,e])){
return;
}
}
};
RadGridNamespace.RadGridTableRow.prototype.SingleSelect=function(){
if(!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
this.Owner.ClearSelectedRows();
this.Owner.Owner.ClearSelectedRows();
this.Selected=true;
this.ApplySelectedRowStyle();
this.Owner.AddToSelectedRows(this);
this.Owner.Owner.UpdateClientRowSelection();
};
RadGridNamespace.RadGridTableRow.prototype.SingleDeselect=function(){
if(!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect){
return;
}
this.Owner.ClearSelectedRows();
this.Owner.Owner.ClearSelectedRows();
this.Selected=false;
this.RemoveSelectedRowStyle();
this.Owner.RemoveFromSelectedRows(this);
this.Owner.Owner.UpdateClientRowSelection();
};
RadGridNamespace.RadGridTableRow.prototype.MultiSelect=function(){
if((!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect)||(!this.Owner.Owner.AllowMultiRowSelection)){
return;
}
if(this.Selected){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowDeselecting",[this])){
return;
}
this.Selected=false;
this.RemoveSelectedRowStyle();
this.Owner.RemoveFromSelectedRows(this);
this.Owner.Owner.UpdateClientRowSelection();
}else{
this.Selected=true;
this.ApplySelectedRowStyle();
this.Owner.AddToSelectedRows(this);
this.Owner.Owner.UpdateClientRowSelection();
}
};
RadGridNamespace.RadGridTableRow.prototype.LoadSelected=function(){
this.ApplySelectedRowStyle();
this.Owner.AddToSelectedRows(this);
};
RadGridNamespace.RadGridTableRow.prototype.ApplySelectedRowStyle=function(){
if(!this.Owner.SelectedItemStyleClass||this.Owner.SelectedItemStyleClass==""){
if(this.Owner.SelectedItemStyle&&this.Owner.SelectedItemStyle!=""){
RadGridNamespace.addClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"1");
}else{
RadGridNamespace.addClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"2");
}
}else{
RadGridNamespace.addClassName(this.Control,this.Owner.SelectedItemStyleClass);
}
};
RadGridNamespace.RadGridTableRow.prototype.RemoveSelectedRowStyle=function(){
if(this.Owner.SelectedItemStyle){
RadGridNamespace.removeClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"1");
}else{
RadGridNamespace.removeClassName(this.Control,"SelectedItemStyle"+this.Owner.ClientID+"2");
}
RadGridNamespace.removeClassName(this.Control,this.Owner.SelectedItemStyleClass);
if(this.Control.style.cssText==this.Owner.SelectedItemStyle){
this.Control.style.cssText="";
}
};
RadGridNamespace.RadGridTableRow.prototype.OnDblClick=function(e){
try{
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowDblClick",[this.Control.sectionRowIndex,e])){
return;
}
}
catch(error){
new RadGridNamespace.Error(error,this,this.Owner.Owner.OnError);
}
};
RadGridNamespace.RadGridTableRow.prototype.CreateRowSelectorArea=function(e){
if((this.Owner.Owner.RowResizer)||(e.ctrlKey)){
return;
}
var _294=null;
if(e.srcElement){
_294=e.srcElement;
}else{
if(e.target){
_294=e.target;
}
}
if(!_294.tagName){
return;
}
if(_294.tagName.toLowerCase()=="input"||_294.tagName.toLowerCase()=="textarea"){
return;
}
if((!this.Owner.Owner.ClientSettings.Selecting.AllowRowSelect)||(!this.Owner.Owner.AllowMultiRowSelection)){
return;
}
var _295=RadGridNamespace.GetCurrentElement(e);
if((!_295)||(!RadGridNamespace.IsChildOf(_295,this.Control))){
return;
}
if(!this.RowSelectorArea){
this.RowSelectorArea=document.createElement("span");
this.RowSelectorArea.style.backgroundColor="navy";
this.RowSelectorArea.style.border="indigo 1px solid";
this.RowSelectorArea.style.position="absolute";
this.RowSelectorArea.style.font="icon";
if(window.netscape&&!window.opera){
this.RowSelectorArea.style.MozOpacity=1/10;
}else{
if(window.opera||navigator.userAgent.indexOf("Safari")>-1){
this.RowSelectorArea.style.opacity=0.1;
}else{
this.RowSelectorArea.style.filter="alpha(opacity=10);";
}
}
if(this.Owner.Owner.GridDataDiv){
this.RowSelectorArea.style.top=RadGridNamespace.FindPosY(this.Control)-this.Owner.Owner.GridDataDiv.scrollTop+"px";
this.RowSelectorArea.style.left=RadGridNamespace.FindPosX(this.Control)-this.Owner.Owner.GridDataDiv.scrollLeft+"px";
if(parseInt(this.RowSelectorArea.style.left)<RadGridNamespace.FindPosX(this.Owner.Owner.Control)){
this.RowSelectorArea.style.left=RadGridNamespace.FindPosX(this.Owner.Owner.Control)+"px";
}
}else{
this.RowSelectorArea.style.top=RadGridNamespace.FindPosY(this.Control)+"px";
this.RowSelectorArea.style.left=RadGridNamespace.FindPosX(this.Control)+"px";
}
document.body.appendChild(this.RowSelectorArea);
this.FirstRow=this.Control;
RadGridNamespace.ClearDocumentEvents();
}
};
RadGridNamespace.RadGridTableRow.prototype.DestroyRowSelectorArea=function(e){
if(this.RowSelectorArea){
var _297=this.RowSelectorArea.style.height;
document.body.removeChild(this.RowSelectorArea);
this.RowSelectorArea=null;
RadGridNamespace.RestoreDocumentEvents();
var _298=RadGridNamespace.GetCurrentElement(e);
var _299;
if((!_298)||(!RadGridNamespace.IsChildOf(_298,this.Owner.Control))){
return;
}
var _29a=RadGridNamespace.GetFirstParentByTagName(_298,"td");
if((_298.tagName.toLowerCase()=="td")||(_298.tagName.toLowerCase()=="tr")||_29a.tagName.toLowerCase()=="td"){
if(_298.tagName.toLowerCase()=="td"){
_299=_298.parentNode;
}else{
if(_29a.tagName.toLowerCase()=="td"){
_299=_29a.parentNode;
}else{
if(_298.tagName.toLowerCase()=="tr"){
_299=_298;
}
}
}
for(var i=this.FirstRow.rowIndex;i<_299.rowIndex+1;i++){
var _29c=this.Owner.Owner.GetRowObjectByRealRow(this.Owner,this.Owner.Control.rows[i]);
if(_29c){
if(_297!=""){
if(!_29c.Selected){
this.Owner.SelectRow(this.Owner.Control.rows[i],false);
}
}
}
}
}
}
};
RadGridNamespace.RadGridTableRow.prototype.ResizeRowSelectorArea=function(e){
if((this.RowSelectorArea)&&(this.RowSelectorArea.parentNode)){
var _29e=RadGridNamespace.GetCurrentElement(e);
if((!_29e)||(!RadGridNamespace.IsChildOf(_29e,this.Owner.Control))){
return;
}
var _29f=parseInt(this.RowSelectorArea.style.left);
if(this.Owner.Owner.GridDataDiv){
var _2a0=RadGridNamespace.GetEventPosX(e)-this.Owner.Owner.GridDataDiv.scrollLeft;
}else{
var _2a0=RadGridNamespace.GetEventPosX(e);
}
var _2a1=parseInt(this.RowSelectorArea.style.top);
if(this.Owner.Owner.GridDataDiv){
var _2a2=RadGridNamespace.GetEventPosY(e)-this.Owner.Owner.GridDataDiv.scrollTop;
}else{
var _2a2=RadGridNamespace.GetEventPosY(e);
}
if((_2a0-_29f-5)>0){
this.RowSelectorArea.style.width=_2a0-_29f-5+"px";
}
if((_2a2-_2a1-5)>0){
this.RowSelectorArea.style.height=_2a2-_2a1-5+"px";
}
if(this.RowSelectorArea.offsetWidth>this.Owner.Control.offsetWidth){
this.RowSelectorArea.style.width=this.Owner.Control.offsetWidth+"px";
}
var _2a3=(RadGridNamespace.FindPosX(this.Owner.Control)+this.Owner.Control.offsetHeight)-parseInt(this.RowSelectorArea.style.top);
if(this.RowSelectorArea.offsetHeight>_2a3){
if(_2a3>0){
this.RowSelectorArea.style.height=_2a3+"px";
}
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseDown=function(e){
if(this.Owner.Owner.ClientSettings.Selecting.EnableDragToSelectRows&&this.Owner.Owner.AllowMultiRowSelection){
if(!this.Owner.Owner.RowResizer){
this.CreateRowSelectorArea(e);
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseUp=function(e){
this.DestroyRowSelectorArea(e);
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseMove=function(e){
this.ResizeRowSelectorArea(e);
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseOver=function(e){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowMouseOver",[this.Control.sectionRowIndex,e])){
return;
}
if(this.Owner.Owner.ClientSettings.EnableRowHoverStyle&&(this.ItemType=="Item"||this.ItemType=="AlternatingItem")){
if(this.Owner.Owner.Skin!=""&&this.Owner.Owner.Skin!="None"){
RadGridNamespace.addClassName(this.Control,"GridRowOver_"+this.Owner.Owner.Skin);
}
}
};
RadGridNamespace.RadGridTableRow.prototype.OnMouseOut=function(e){
if(!RadGridNamespace.FireEvent(this.Owner,"OnRowMouseOut",[this.Control.sectionRowIndex,e])){
return;
}
if(this.Owner.Owner.ClientSettings.EnableRowHoverStyle&&(this.ItemType=="Item"||this.ItemType=="AlternatingItem")){
if(this.Owner.Owner.Skin!=""&&this.Owner.Owner.Skin!="None"){
RadGridNamespace.removeClassName(this.Control,"GridRowOver_"+this.Owner.Owner.Skin);
}
}
};
RadGridNamespace.RadGridGroupPanel=function(_2a9,_2aa){
this.Control=_2a9;
this.Owner=_2aa;
this.Items=new Array();
this.groupPanelItemCounter=0;
this.getGroupPanelItems(this.Control,0);
var _2ab=this;
};
RadGridNamespace.RadGridGroupPanel.prototype.Dispose=function(){
this.UnLoadHandler=null;
this.Control=null;
this.Owner=null;
this.DisposeItems();
for(var _2ac in this){
this[_2ac]=null;
}
};
RadGridNamespace.RadGridGroupPanel.prototype.DisposeItems=function(){
if(this.Items!=null){
for(var i=0;i<this.Items.length;i++){
var item=this.Items[i];
item.Dispose();
}
}
};
RadGridNamespace.RadGridGroupPanel.prototype.groupPanelItemCounter=0;
RadGridNamespace.RadGridGroupPanel.prototype.getGroupPanelItems=function(_2af){
for(var i=0;i<_2af.rows.length;i++){
var _2b1=false;
var row=_2af.rows[i];
for(var j=0;j<row.cells.length;j++){
var cell=row.cells[j];
if(cell.tagName.toLowerCase()=="th"){
var _2b5;
if(this.Owner.GroupPanel.GroupPanelItems[this.groupPanelItemCounter]){
_2b5=this.Owner.GroupPanel.GroupPanelItems[this.groupPanelItemCounter].HierarchicalIndex;
}
if(_2b5){
this.Items[this.Items.length]=new RadGridNamespace.RadGridGroupPanelItem(cell,this,_2b5);
_2b1=true;
this.groupPanelItemCounter++;
}
}
if((cell.firstChild)&&(cell.firstChild.tagName)){
if(cell.firstChild.tagName.toLowerCase()=="table"){
this.getGroupPanelItems(cell.firstChild);
}
}
}
}
};
RadGridNamespace.RadGridGroupPanel.prototype.IsItem=function(_2b6){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Control==_2b6){
return this.Items[i];
}
}
return null;
};
RadGridNamespace.RadGridGroupPanelItem=function(_2b8,_2b9,_2ba){
RadControlsNamespace.DomEventMixin.Initialize(this);
this.Control=_2b8;
this.Owner=_2b9;
this.HierarchicalIndex=_2ba;
this.Control.style.cursor="move";
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
};
RadGridNamespace.RadGridGroupPanelItem.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
for(var _2bb in this){
this[_2bb]=null;
}
this.Control=null;
this.Owner=null;
};
RadGridNamespace.RadGridGroupPanelItem.prototype.OnMouseDown=function(e){
if(((window.netscape||window.opera||navigator.userAgent.indexOf("Safari")!=-1)&&(e.button==0))||(e.button==1)){
this.CreateDragDrop(e);
this.CreateReorderIndicators(this.Control);
this.AttachDomEvent(document,"mouseup","OnMouseUp");
this.AttachDomEvent(document,"mousemove","OnMouseMove");
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.OnMouseUp=function(e){
this.FireDropAction(e);
this.DestroyDragDrop(e);
this.DestroyReorderIndicators();
this.DetachDomEvent(document,"mouseup","OnMouseUp");
this.DetachDomEvent(document,"mousemove","OnMouseMove");
};
RadGridNamespace.RadGridGroupPanelItem.prototype.OnMouseMove=function(e){
this.MoveDragDrop(e);
};
RadGridNamespace.RadGridGroupPanelItem.prototype.FireDropAction=function(e){
var _2c0=RadGridNamespace.GetCurrentElement(e);
if(_2c0!=null){
if(!RadGridNamespace.IsChildOf(_2c0,this.Owner.Control)){
this.Owner.Owner.SavePostData("UnGroupByExpression",this.HierarchicalIndex);
eval(this.Owner.Owner.ClientSettings.PostBackReferences.PostBackUnGroupByExpression);
}else{
var item=this.Owner.IsItem(_2c0);
if((_2c0!=this.Control)&&(item!=null)&&(_2c0.parentNode==this.Control.parentNode)){
this.Owner.Owner.SavePostData("ReorderGroupByExpression",this.HierarchicalIndex,item.HierarchicalIndex);
eval(this.Owner.Owner.ClientSettings.PostBackReferences.PostBackReorderGroupByExpression);
}
if(window.netscape){
this.Control.style.MozOpacity=4/4;
}else{
this.Control.style.filter="alpha(opacity=100);";
}
}
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.CreateDragDrop=function(e){
this.MoveHeaderDiv=document.createElement("div");
var _2c3=document.createElement("table");
if(this.MoveHeaderDiv.mergeAttributes){
this.MoveHeaderDiv.mergeAttributes(this.Owner.Owner.Control);
}else{
RadGridNamespace.CopyAttributes(this.MoveHeaderDiv,this.Control);
}
if(_2c3.mergeAttributes){
_2c3.mergeAttributes(this.Owner.Control);
}else{
RadGridNamespace.CopyAttributes(_2c3,this.Owner.Control);
}
_2c3.style.margin="0px";
_2c3.style.height=this.Control.offsetHeight+"px";
_2c3.style.width=this.Control.offsetWidth+"px";
_2c3.style.border="0px";
_2c3.style.borderCollapse="collapse";
_2c3.style.padding="0px";
var _2c4=document.createElement("thead");
var tr=document.createElement("tr");
_2c3.appendChild(_2c4);
_2c4.appendChild(tr);
tr.appendChild(this.Control.cloneNode(true));
this.MoveHeaderDiv.appendChild(_2c3);
document.body.appendChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.style.height=_2c3.style.height;
this.MoveHeaderDiv.style.width=_2c3.style.width;
this.MoveHeaderDiv.style.position="absolute";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
if(window.netscape){
this.MoveHeaderDiv.style.MozOpacity=3/4;
}else{
this.MoveHeaderDiv.style.filter="alpha(opacity=75);";
}
this.MoveHeaderDiv.style.cursor="move";
this.MoveHeaderDiv.style.display="none";
this.MoveHeaderDiv.onmousedown=null;
RadGridNamespace.ClearDocumentEvents();
};
RadGridNamespace.RadGridGroupPanelItem.prototype.DestroyDragDrop=function(e){
if(this.MoveHeaderDiv!=null){
var _2c7=this.MoveHeaderDiv.parentNode;
_2c7.removeChild(this.MoveHeaderDiv);
this.MoveHeaderDiv.onmouseup=null;
this.MoveHeaderDiv.onmousemove=null;
this.MoveHeaderDiv=null;
RadGridNamespace.RestoreDocumentEvents();
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.MoveDragDrop=function(e){
if(this.MoveHeaderDiv!=null){
if(window.netscape){
this.Control.style.MozOpacity=1/4;
}else{
this.Control.style.filter="alpha(opacity=25);";
}
this.MoveHeaderDiv.style.visibility="";
this.MoveHeaderDiv.style.display="";
RadGridNamespace.RadGrid.PositionDragElement(this.MoveHeaderDiv,e);
var _2c9=RadGridNamespace.GetCurrentElement(e);
if(_2c9!=null){
if(RadGridNamespace.IsChildOf(_2c9,this.Owner.Control)){
var item=this.Owner.IsItem(_2c9);
if((_2c9!=this.Control)&&(item!=null)&&(_2c9.parentNode==this.Control.parentNode)){
this.MoveReorderIndicators(e,_2c9);
}else{
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
}
}
}
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.CreateReorderIndicators=function(_2cb){
if((this.ReorderIndicator1==null)&&(this.ReorderIndicator2==null)){
this.ReorderIndicator1=document.createElement("span");
this.ReorderIndicator2=document.createElement("span");
if(this.Owner.Owner.Skin==""||this.Owner.Owner.Skin=="None"){
this.ReorderIndicator1.innerHTML="&darr;";
this.ReorderIndicator2.innerHTML="&uarr;";
}else{
this.ReorderIndicator1.className="TopReorderIndicator_"+this.Owner.Owner.Skin;
this.ReorderIndicator2.className="BottomReorderIndicator_"+this.Owner.Owner.Skin;
this.ReorderIndicator1.style.width=this.ReorderIndicator1.style.height=this.ReorderIndicator2.style.width=this.ReorderIndicator2.style.height="10px";
}
this.ReorderIndicator1.style.backgroundColor="transparent";
this.ReorderIndicator1.style.color="darkblue";
this.ReorderIndicator1.style.font="bold 18px Arial";
this.ReorderIndicator2.style.backgroundColor=this.ReorderIndicator1.style.backgroundColor;
this.ReorderIndicator2.style.color=this.ReorderIndicator1.style.color;
this.ReorderIndicator2.style.font=this.ReorderIndicator1.style.font;
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_2cb)-this.ReorderIndicator1.offsetHeight+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_2cb)+"px";
this.ReorderIndicator2.style.top=RadGridNamespace.FindPosY(_2cb)+_2cb.offsetHeight+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
this.ReorderIndicator1.style.visibility="hidden";
this.ReorderIndicator1.style.display="none";
this.ReorderIndicator1.style.position="absolute";
this.ReorderIndicator2.style.visibility=this.ReorderIndicator1.style.visibility;
this.ReorderIndicator2.style.display=this.ReorderIndicator1.style.display;
this.ReorderIndicator2.style.position=this.ReorderIndicator1.style.position;
document.body.appendChild(this.ReorderIndicator1);
document.body.appendChild(this.ReorderIndicator2);
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.DestroyReorderIndicators=function(){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
document.body.removeChild(this.ReorderIndicator1);
document.body.removeChild(this.ReorderIndicator2);
this.ReorderIndicator1=null;
this.ReorderIndicator2=null;
}
};
RadGridNamespace.RadGridGroupPanelItem.prototype.MoveReorderIndicators=function(e,_2cd){
if((this.ReorderIndicator1!=null)&&(this.ReorderIndicator2!=null)){
this.ReorderIndicator1.style.visibility="visible";
this.ReorderIndicator1.style.display="";
this.ReorderIndicator2.style.visibility="visible";
this.ReorderIndicator2.style.display="";
this.ReorderIndicator1.style.top=RadGridNamespace.FindPosY(_2cd)-this.ReorderIndicator1.offsetHeight+"px";
this.ReorderIndicator1.style.left=RadGridNamespace.FindPosX(_2cd)+"px";
this.ReorderIndicator2.style.top=RadGridNamespace.FindPosY(_2cd)+_2cd.offsetHeight+"px";
this.ReorderIndicator2.style.left=this.ReorderIndicator1.style.left;
}
};
RadGridNamespace.RadGridMenu=function(_2ce,_2cf,_2d0){
if(!_2ce||!_2cf){
return;
}
RadControlsNamespace.DomEventMixin.Initialize(this);
for(var _2d1 in _2ce){
this[_2d1]=_2ce[_2d1];
}
this.Owner=_2cf;
this.ItemData=_2ce.Items;
this.Items=[];
};
RadGridNamespace.RadGridMenu.prototype.Initialize=function(){
if(this.Control!=null){
return;
}
this.Control=document.createElement("table");
this.Control.style.backgroundColor=this.SelectColumnBackColor;
this.Control.style.border="outset 1px";
this.Control.style.fontSize="small";
this.Control.style.textAlign="left";
this.Control.cellPadding="0";
this.Control.style.borderCollapse="collapse";
this.Control.style.zIndex=998;
this.Skin=(this.Owner&&this.Owner.Owner&&this.Owner.Owner.Skin)||"None";
var _2d2=RadGridNamespace.IsRightToLeft(this.Owner.Control);
if(_2d2){
this.Control.style.direction="rtl";
RadGridNamespace.addClassName(this.Control,"RadGridRTL_"+this.Skin);
}
RadGridNamespace.addClassName(this.Control,"GridFilterMenu_"+this.Skin);
RadGridNamespace.addClassName(this.Control,this.CssClass);
this.Items=this.CreateItems(this.ItemData);
this.Control.style.position="absolute";
this.Control.style.display="none";
document.body.appendChild(this.Control);
var _2d3=document.createElement("img");
_2d3.src=this.SelectedImageUrl;
_2d3.src=this.NotSelectedImageUrl;
this.Control.style.zIndex=100000;
};
RadGridNamespace.RadGridMenu.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
this.DisposeItems();
this.ItemData=null;
this.Owner=null;
this.Control=null;
};
RadGridNamespace.RadGridMenu.prototype.CreateItems=function(_2d4){
var _2d5=[];
for(var i=0;i<_2d4.length;i++){
_2d5[_2d5.length]=new RadGridNamespace.RadGridMenuItem(_2d4[i],this);
}
return _2d5;
};
RadGridNamespace.RadGridMenu.prototype.DisposeItems=function(){
for(var i=0;i<this.Items.length;i++){
var item=this.Items[i];
item.Dispose();
}
this.Items=null;
};
RadGridNamespace.RadGridMenu.prototype.HideItem=function(_2d9){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Value==_2d9){
this.Items[i].Control.style.display="none";
}
}
};
RadGridNamespace.RadGridMenu.prototype.ShowItem=function(_2db){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Value==_2db){
this.Items[i].Control.style.display="";
}
}
};
RadGridNamespace.RadGridMenu.prototype.SelectItem=function(_2dd){
for(var i=0;i<this.Items.length;i++){
if(this.Items[i].Value==_2dd){
this.Items[i].Selected=true;
this.Items[i].SelectImage.src=this.SelectedImageUrl;
}else{
this.Items[i].Selected=false;
this.Items[i].SelectImage.src=this.NotSelectedImageUrl;
}
}
};
RadGridNamespace.RadGridMenu.prototype.Show=function(_2df,_2e0,e){
this.Initialize();
this.Control.style.display="";
this.Control.style.top=e.clientY+document.documentElement.scrollTop+document.body.scrollTop+5+"px";
this.Control.style.left=e.clientX+document.documentElement.scrollLeft+document.body.scrollLeft+5+"px";
this.AttachHideEvents();
};
RadGridNamespace.RadGridMenu.prototype.OnKeyPress=function(e){
if(e.keyCode==27){
this.DetachHideEvents();
this.Hide();
}
};
RadGridNamespace.RadGridMenu.prototype.OnClick=function(e){
if(!e.cancelBubble){
this.DetachHideEvents();
this.Hide();
}
};
RadGridNamespace.RadGridMenu.prototype.AttachHideEvents=function(){
this.AttachDomEvent(document,"keypress","OnKeyPress");
this.AttachDomEvent(document,"click","OnClick");
};
RadGridNamespace.RadGridMenu.prototype.DetachHideEvents=function(){
this.DetachDomEvent(document,"keypress","OnKeyPress");
this.DetachDomEvent(document,"click","OnClick");
};
RadGridNamespace.RadGridMenu.prototype.Hide=function(){
if(this.Control.style.display==""){
this.Control.style.display="none";
}
};
RadGridNamespace.RadGridMenuItem=function(_2e4,_2e5){
for(var _2e6 in _2e4){
this[_2e6]=_2e4[_2e6];
}
this.Owner=_2e5;
this.Skin=this.Owner.Skin;
this.Control=this.Owner.Control.insertRow(-1);
this.Control.insertCell(-1);
var _2e7=document.createElement("table");
_2e7.style.width="100%";
_2e7.cellPadding="0";
_2e7.cellSpacing="0";
_2e7.insertRow(-1);
var td1=_2e7.rows[0].insertCell(-1);
var td2=_2e7.rows[0].insertCell(-1);
if(this.Skin=="None"){
td1.style.borderTop="solid 1px "+this.Owner.SelectColumnBackColor;
td1.style.borderLeft="solid 1px "+this.Owner.SelectColumnBackColor;
td1.style.borderRight="none 0px";
td1.style.borderBottom="solid 1px "+this.Owner.SelectColumnBackColor;
td1.style.padding="2px";
td1.style.textAlign="center";
}else{
RadGridNamespace.addClassName(td1,"GridFilterMenuSelectColumn_"+this.Skin);
}
td1.style.width="16px";
td1.appendChild(document.createElement("img"));
td1.childNodes[0].src=this.Owner.NotSelectedImageUrl;
this.SelectImage=td1.childNodes[0];
if(this.Skin=="None"){
td2.style.borderTop="solid 1px "+this.Owner.TextColumnBackColor;
td2.style.borderLeft="none 0px";
td2.style.borderRight="solid 1px "+this.Owner.TextColumnBackColor;
td2.style.borderBottom="solid 1px "+this.Owner.TextColumnBackColor;
td2.style.padding="2px";
td2.style.backgroundColor=this.Owner.TextColumnBackColor;
td2.style.cursor="pointer";
}else{
RadGridNamespace.addClassName(td2,"GridFilterMenuTextColumn_"+this.Skin);
}
td2.innerHTML=this.Text;
this.Control.cells[0].appendChild(_2e7);
var _2ea=this;
this.Control.onclick=function(){
if(_2ea.Owner.Owner.Owner.EnableAJAX){
if(_2ea.Owner.Owner==_2ea.Owner.Owner.Owner.MasterTableViewHeader){
RadGridNamespace.AsyncRequest(_2ea.UID,_2ea.Owner.Owner.Owner.MasterTableView.UID+"!"+_2ea.Owner.Column.UniqueName,_2ea.Owner.Owner.Owner.ClientID);
}else{
RadGridNamespace.AsyncRequest(_2ea.UID,_2ea.Owner.Owner.UID+"!"+_2ea.Owner.Column.UniqueName,_2ea.Owner.Owner.Owner.ClientID);
}
}else{
var _2eb=_2ea.Owner.Owner.Owner.ClientSettings.PostBackFunction;
if(_2ea.Owner.Owner==_2ea.Owner.Owner.Owner.MasterTableViewHeader){
_2eb=_2eb.replace("{0}",_2ea.UID).replace("{1}",_2ea.Owner.Owner.Owner.MasterTableView.UID+"!"+_2ea.Owner.Column.UniqueName);
}else{
_2eb=_2eb.replace("{0}",_2ea.UID).replace("{1}",_2ea.Owner.Owner.UID+"!"+_2ea.Owner.Column.UniqueName);
}
eval(_2eb);
}
};
var _2ea=this;
this.Control.onmouseover=function(e){
if(_2ea.Skin=="None"){
this.cells[0].childNodes[0].rows[0].cells[0].style.backgroundColor=_2ea.Owner.HoverBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderTop="solid 1px "+_2ea.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderLeft="solid 1px "+_2ea.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderBottom="solid 1px "+_2ea.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.backgroundColor=_2ea.Owner.HoverBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderTop="solid 1px "+_2ea.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderRight="solid 1px "+_2ea.Owner.HoverBorderColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderBottom="solid 1px "+_2ea.Owner.HoverBorderColor;
}else{
RadGridNamespace.addClassName(this.cells[0].childNodes[0].rows[0].cells[0],"GridFilterMenuHover_"+_2ea.Skin);
RadGridNamespace.addClassName(this.cells[0].childNodes[0].rows[0].cells[1],"GridFilterMenuHover_"+_2ea.Skin);
}
};
this.Control.onmouseout=function(e){
if(_2ea.Skin=="None"){
this.cells[0].childNodes[0].rows[0].cells[0].style.borderTop="solid 1px "+_2ea.Owner.SelectColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderLeft="solid 1px "+_2ea.Owner.SelectColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.borderBottom="solid 1px "+_2ea.Owner.SelectColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[0].style.backgroundColor="";
this.cells[0].childNodes[0].rows[0].cells[1].style.borderTop="solid 1px "+_2ea.Owner.TextColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderRight="solid 1px "+_2ea.Owner.TextColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.borderBottom="solid 1px "+_2ea.Owner.TextColumnBackColor;
this.cells[0].childNodes[0].rows[0].cells[1].style.backgroundColor=_2ea.Owner.TextColumnBackColor;
}else{
RadGridNamespace.removeClassName(this.cells[0].childNodes[0].rows[0].cells[0],"GridFilterMenuHover_"+_2ea.Skin);
RadGridNamespace.removeClassName(this.cells[0].childNodes[0].rows[0].cells[1],"GridFilterMenuHover_"+_2ea.Skin);
}
};
};
RadGridNamespace.RadGridMenuItem.prototype.Dispose=function(){
this.Control.onclick=null;
this.Control.onmouseover=null;
this.Control.onmouseout=null;
var _2ee=this.Control.getElementsByTagName("table");
while(_2ee.length>0){
var _2ef=_2ee[0];
if(_2ef.parentNode!=null){
_2ef.parentNode.removeChild(_2ef);
}
}
this.Control=null;
this.Owner=null;
};
RadGridNamespace.RadGridFilterMenu=function(_2f0,_2f1){
RadGridNamespace.RadGridMenu.call(this,_2f0,_2f1);
};
RadGridNamespace.RadGridFilterMenu.prototype=new RadGridNamespace.RadGridMenu;
RadGridNamespace.RadGridFilterMenu.prototype.Show=function(_2f2,e){
this.Initialize();
if(!_2f2){
return;
}
this.Owner=_2f2.Owner;
this.Column=_2f2;
for(var i=0;i<this.Items.length;i++){
if(_2f2.DataTypeName=="System.Boolean"){
if((this.Items[i].Value=="GreaterThan")||(this.Items[i].Value=="LessThan")||(this.Items[i].Value=="GreaterThanOrEqualTo")||(this.Items[i].Value=="LessThanOrEqualTo")||(this.Items[i].Value=="Between")||(this.Items[i].Value=="NotBetween")){
this.Items[i].Control.style.display="none";
continue;
}
}
if(_2f2.DataTypeName!="System.String"){
if((this.Items[i].Value=="StartsWith")||(this.Items[i].Value=="EndsWith")||(this.Items[i].Value=="Contains")||(this.Items[i].Value=="DoesNotContain")||(this.Items[i].Value=="IsEmpty")||(this.Items[i].Value=="NotIsEmpty")){
this.Items[i].Control.style.display="none";
continue;
}
}
if(_2f2.FilterListOptions=="VaryByDataType"){
if(this.Items[i].Value=="Custom"){
this.Items[i].Control.style.display="none";
continue;
}
}
this.Items[i].Control.style.display="";
}
this.SelectItem(_2f2.CurrentFilterFunction);
var args={Menu:this,TableView:this.Owner,Column:this.Column,Event:e};
if(!RadGridNamespace.FireEvent(this.Owner,"OnFilterMenuShowing",[this.Owner,args])){
return;
}
this.Control.style.display="";
this.Control.style.top=e.clientY+document.documentElement.scrollTop+document.body.scrollTop+5+"px";
this.Control.style.left=e.clientX+document.documentElement.scrollLeft+document.body.scrollLeft+5+"px";
this.AttachHideEvents();
};
RadGridNamespace.RadGrid.prototype.InitializeFilterMenu=function(_2f6){
if(this.AllowFilteringByColumn||_2f6.AllowFilteringByColumn){
if(!_2f6||!_2f6.Control){
return;
}
if(!_2f6.Control.tHead){
return;
}
if(!_2f6.IsItemInserted){
var _2f7=_2f6.Control.tHead.rows[_2f6.Control.tHead.rows.length-1];
}else{
var _2f7=_2f6.Control.tHead.rows[_2f6.Control.tHead.rows.length-2];
}
if(!_2f7){
return;
}
var _2f8=_2f7.getElementsByTagName("img");
var _2f9=this;
if(!_2f6.Columns){
return;
}
if(!_2f6.Columns[0]){
return;
}
var _2fa=_2f6.Columns[0].FilterImageUrl;
for(var i=0;i<_2f8.length;i++){
var _2fc=RadGridNamespace.EncodeURI(_2fa);
if(_2f8[i].getAttribute("src").indexOf(_2fc)==-1){
continue;
}
_2f8[i].onclick=function(e){
if(!e){
var e=window.event;
}
e.cancelBubble=true;
var _2fe=this.parentNode.cellIndex;
if(window.attachEvent&&!window.opera&&!window.netscape){
_2fe=RadGridNamespace.GetRealCellIndexFormCells(this.parentNode.parentNode.cells,this.parentNode);
}
_2f9.FilteringMenu.Show(_2f6.Columns[_2fe],e);
if(e.preventDefault){
e.preventDefault();
}else{
e.returnValue=false;
return false;
}
};
}
this.FilteringMenu=new RadGridNamespace.RadGridFilterMenu(this.FilterMenu,_2f6);
}
};
RadGridNamespace.RadGrid.prototype.DisposeFilterMenu=function(_2ff){
if(this.FilteringMenu!=null){
this.FilteringMenu.Dispose();
this.FilteringMenu=null;
}
};
RadGridNamespace.GetRealCellIndexFormCells=function(_300,cell){
for(var i=0;i<_300.length;i++){
if(_300[i]==cell){
return i;
}
}
};
if(typeof (window.RadGridNamespace)=="undefined"){
window.RadGridNamespace=new Object();
}
RadGridNamespace.Slider=function(_303){
RadControlsNamespace.DomEventMixin.Initialize(this);
if(!document.readyState||document.readyState=="complete"||window.opera){
this._constructor(_303);
}else{
this.objectData=_303;
this.AttachDomEvent(window,"load","OnWindowLoad");
}
};
RadGridNamespace.Slider.prototype.OnWindowLoad=function(e){
this.DetachDomEvent(window,"load","OnWindowLoad");
this._constructor(this.objectData);
this.objectData=null;
};
RadGridNamespace.Slider.prototype._constructor=function(_305){
var _306=this;
for(var _307 in _305){
this[_307]=_305[_307];
}
this.Owner=window[this.OwnerID];
this.OwnerGrid=window[this.OwnerGridID];
this.Control=document.getElementById(this.ClientID);
if(this.Control==null){
return;
}
this.Control.unselectable="on";
this.Control.parentNode.style.padding="10px";
this.ToolTip=document.createElement("div");
this.ToolTip.unselectable="on";
this.ToolTip.style.backgroundColor="#F5F5DC";
this.ToolTip.style.border="1px outset";
this.ToolTip.style.font="icon";
this.ToolTip.style.padding="2px";
this.ToolTip.style.marginTop="5px";
this.ToolTip.style.marginBottom="15px";
this.Control.appendChild(this.ToolTip);
this.Line=document.createElement("hr");
this.Line.unselectable="on";
this.Line.style.width="100%";
this.Line.style.height="2px";
this.Line.style.backgroundColor="buttonface";
this.Line.style.border="1px outset threedshadow";
this.Control.appendChild(this.Line);
this.Thumb=document.createElement("div");
this.Thumb.unselectable="on";
this.Thumb.style.position="relative";
this.Thumb.style.width="8px";
this.Thumb.style.marginTop="-15px";
this.Thumb.style.height="16px";
this.Thumb.style.backgroundColor="buttonface";
this.Thumb.style.border="1px outset threedshadow";
this.Control.appendChild(this.Thumb);
this.Link=document.createElement("a");
this.Link.unselectable="on";
this.Link.style.width="100%";
this.Link.style.height="100%";
this.Link.style.display="block";
this.Link.href="javascript:void(0);";
this.Thumb.appendChild(this.Link);
this.LineX=RadGridNamespace.FindPosX(this.Line);
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
this.AttachDomEvent(this.Link,"keydown","OnKeyDown");
var _308=this.OwnerGrid.CurrentPageIndex/this.OwnerGrid.MasterTableView.PageCount;
this.SetPosition(_308*this.Line.offsetWidth);
var _309=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _30a=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_309);
this.OwnerGrid.ApplyPagerTooltipText(this.ToolTip,this.OwnerGrid.CurrentPageIndex,this.OwnerGrid.MasterTableView.PageCount);
};
RadGridNamespace.Slider.prototype.Dispose=function(){
this.DisposeDomEventHandlers();
for(var _30b in this){
this[_30b]=null;
}
this.Control=null;
this.Line=null;
this.Thumb=null;
this.ToolTip=null;
};
RadGridNamespace.Slider.prototype.OnKeyDown=function(e){
this.AttachDomEvent(this.Link,"keyup","OnKeyUp");
if(e.keyCode==39){
this.SetPosition(parseInt(this.Thumb.style.left)+this.Thumb.offsetWidth);
}
if(e.keyCode==37){
this.SetPosition(parseInt(this.Thumb.style.left)-this.Thumb.offsetWidth);
}
if(e.keyCode==39||e.keyCode==37){
var _30d=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _30e=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_30d);
this.OwnerGrid.ApplyPagerTooltipText(this.ToolTip,_30e,this.OwnerGrid.MasterTableView.PageCount);
}
};
RadGridNamespace.Slider.prototype.OnKeyUp=function(e){
this.DetachDomEvent(this.Link,"keyup","OnKeyUp");
if(e.keyCode==39||e.keyCode==37){
var _310=this;
setTimeout(function(){
_310.ChangePage();
},100);
}
};
RadGridNamespace.Slider.prototype.OnMouseDown=function(e){
this.DetachDomEvent(this.Control,"mousedown","OnMouseDown");
if(((window.netscape||window.opera||navigator.userAgent.indexOf("Safari")!=-1))&&(e.button==0)||(e.button==1)){
this.SetPosition(RadGridNamespace.GetEventPosX(e)-this.LineX);
this.AttachDomEvent(document,"mousemove","OnMouseMove");
this.AttachDomEvent(document,"mouseup","OnMouseUp");
}
};
RadGridNamespace.Slider.prototype.OnMouseUp=function(e){
this.DetachDomEvent(document,"mousemove","OnMouseMove");
this.DetachDomEvent(document,"mouseup","OnMouseUp");
var _313=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _314=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_313);
this.OwnerGrid.ApplyPagerTooltipText(this.ToolTip,_314,this.OwnerGrid.MasterTableView.PageCount);
var _315=this;
setTimeout(function(){
_315.ChangePage();
},100);
};
RadGridNamespace.Slider.prototype.OnMouseMove=function(e){
this.SetPosition(RadGridNamespace.GetEventPosX(e)-this.LineX);
var _317=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _318=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_317);
this.OwnerGrid.ApplyPagerTooltipText(this.ToolTip,_318,this.OwnerGrid.MasterTableView.PageCount);
};
RadGridNamespace.Slider.prototype.GetPosition=function(e){
this.SetPosition(RadGridNamespace.GetEventPosX(e)-this.LineX);
};
RadGridNamespace.Slider.prototype.SetPosition=function(_31a){
if(_31a>=0&&_31a<=this.Line.offsetWidth){
this.Thumb.style.left=_31a+"px";
}
};
RadGridNamespace.Slider.prototype.ChangePage=function(){
var _31b=parseInt(this.Thumb.style.left)/this.Line.offsetWidth;
var _31c=Math.round((this.OwnerGrid.MasterTableView.PageCount-1)*_31b);
if(this.OwnerGrid.CurrentPageIndex==_31c){
this.AttachDomEvent(this.Control,"mousedown","OnMouseDown");
return;
}
this.OwnerGrid.SavePostData("AJAXScrolledControl",(this.OwnerGrid.GridDataDiv)?this.OwnerGrid.GridDataDiv.scrollLeft:"",(this.OwnerGrid.GridDataDiv)?this.OwnerGrid.LastScrollTop:"",(this.OwnerGrid.GridDataDiv)?this.OwnerGrid.GridDataDiv.scrollTop:"",_31c);
var _31d=this.OwnerGrid.ClientSettings.PostBackFunction;
_31d=_31d.replace("{0}",this.OwnerGrid.UniqueID);
eval(_31d);
};

//BEGIN_ATLAS_NOTIFY
if (typeof(Sys) != "undefined"){if (Sys.Application != null && Sys.Application.notifyScriptLoaded != null){Sys.Application.notifyScriptLoaded();}}
//END_ATLAS_NOTIFY
