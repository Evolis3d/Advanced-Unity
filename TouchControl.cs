using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour {

	public float rayDistance = 20;

	const float MIN_TIME_TOUCH = 0.2f;

	private float touchTime = 0;

	private GameObject target = null;

	void LateUpdate() {

		//Si hay un objeto seleccionado, aplicamos los gestos
		if (target != null) {
			float pinchAmount = 0;
			Transform _target = target.transform;
			Quaternion desiredRotation = _target.rotation;

			//Pedimos a la clase TouchMovement que compruebe los gestos
			TouchMovement.Calculate();

			//Hacemos zoom
			if (Mathf.Abs(TouchMovement.pinchDistanceDelta) > 0) { 
				pinchAmount = TouchMovement.pinchDistanceDelta;
			}

			//Rotamos el objeto
			if (Mathf.Abs(TouchMovement.turnAngleDelta) > 0) { // rotate
				Vector3 rotationDeg = Vector3.zero;
				rotationDeg.x = TouchMovement.turnAngleDelta;
				desiredRotation *= Quaternion.Euler(rotationDeg);
			}
			
			
			// Aplicamos la rotacion y el zoom deseados
			_target.rotation = desiredRotation;

			_target.position += Vector3.right * pinchAmount;

			//Comprobamos si se hace otro toque para soltar el objeto
			OneTouch(true);
		}
		//Si no hay ningun objeto seleccionado, miramos si se toca alguno
		else 
			OneTouch(false);
	}

	void OneTouch (bool activeTarget) {
		//Si solo hay un toque...
		if (Input.touchCount == 1) {
			Touch touch1 = Input.GetTouch(0);

			//Capturamos el instante en el que comienz el toque, para saber que es un toque corto
			if (touch1.phase == TouchPhase.Began)
				touchTime = Time.time;

			//Si el tiempo trnascurrido durante el toque es inferior al margen de toque corto, seleccionamos o deseleccionamos un objeto
			if (touch1.phase == TouchPhase.Ended && (Time.time - touchTime) < MIN_TIME_TOUCH) {


				//Comprobamos si hay algun objeto cerca
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(touch1.position);

				if (Physics.Raycast(ray, out hit, rayDistance)) {

					Debug.Log (hit.transform.name);

					//Si ya habia un objeto selecionado, lo deseleccionamos
					if (activeTarget){
						target.renderer.material.color  = Color.white;
						target.transform.parent = null;
						target = null;
					}
					//Si no habia ningun objeto seleccionado, lo seleccionamso. 
					else {
						target = hit.transform.gameObject;
						target.transform.parent = this.gameObject.transform;
						target.renderer.material.color = Color.yellow;
					}
				}
			}
		}
	}
}
