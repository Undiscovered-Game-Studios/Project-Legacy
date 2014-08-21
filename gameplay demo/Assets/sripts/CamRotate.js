#pragma strict
var target : Transform;
var distance = 5.0;
var xSpeed = 125.0;
var rightclicked : boolean = false;
 
private var x = 0.0;
 
@script AddComponentMenu("Camera-Control/Mouse Orbit")

function Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    target = transform.parent.gameObject.GetComponent(Transform);
}
 
function Update (){
    if (Input.GetMouseButtonDown(1)){
        rightclicked = true;
        }
    else{
        rightclicked = false;
    }
}
 
function LateUpdate () {
    if (target && rightclicked == true) {
        x += Input.GetAxis("Mouse X") * xSpeed * distance* 0.02;
           var rotation = Quaternion.Euler(0, x, 0);
           var position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
           transform.rotation = rotation;
           transform.position = position;
    }   
}