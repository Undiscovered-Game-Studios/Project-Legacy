#pragma strict

var rmbDownPoint;
var mouseRightDrag : boolean = false;
private var targetPosition : Vector3;
 
// Right Mouse Hold
if (Input.GetMouseButton(1)) {
    Screen.showCursor = false;
    } else {
    Screen.showCursor = true;
}
    //Move mouse left or right with RMB held
if (Input.GetMouseButtonDown(1))
{
    rmbDownPoint = Input.GetAxis;
    mouseRightDrag = true;
}
 
if (mouseRightDrag)
{
    var dragDifference : Vector2 = rmbDownPoint;
    targetPosition += transform.forward * 27; 
    transform.RotateAround(targetPosition,Vector3.up+dragDifference ,50*Time.deltaTime);
}
 
if(Input.GetMouseButtonUp(1))
{
    mouseRightDrag = false;
}