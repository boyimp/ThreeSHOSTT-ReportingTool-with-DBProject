

function openDockForm(dock) {

    if (dock != null) {

        if (dock.get_closed()) {
            var viewPort = $telerik.getViewPortSize();
            var xPos = Math.round((viewPort.width - parseInt(dock.get_width())) / 2);
            var yPos = (window.pageYOffset ? window.pageYOffset : document.documentElement.scrollTop) + 150; //Math.round((viewPort.height - parseInt(dock.get_height())) / 2);
            $telerik.setLocation(dock.get_element(), { x: xPos, y: yPos });
            dock.set_closed(false);
        }
        else {
            dock.set_closed(true);
        }
    }

}

function getDisplayTime() {
    return 2500;
}