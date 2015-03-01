using UnityEngine;
using System.Collections;

public class TouchMovement : MonoBehaviour {
	
	const float PINCH_TURN_RATIO = Mathf.PI / 2;
	const float MIN_TURN_ANGLE = 0;
	
	const float PINCH_RATIO = 1;
	const float MIN_PINCH_DISTANCE = 2;
	
	//   La variación del angulo entre dos puntos de toque
	static public float turnAngleDelta;
	
	//   El ángulo entre dos puntos de toque
	static public float turnAngle;
	
	//   La variación de la distancia entre dos puntos de toque que se alejan
	static public float pinchDistanceDelta;
	
	//   La distancia entre dos puntos de toque que se alejan
	static public float pinchDistance;
	
	//   Calculamos el Pinch y el Rotate.
	static public void Calculate () {
		pinchDistance = pinchDistanceDelta = 0;
		turnAngle = turnAngleDelta = 0;
		
		// Si hay dos toques...
		if (Input.touchCount == 2) {
			Touch touch1 = Input.touches[0];
			Touch touch2 = Input.touches[1];
			
			// Y al menos uno de ellos se está moviendo...
			if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved) {
				// Comprobamos la variación en la distancia entre ellos...
				pinchDistance = Vector2.Distance(touch1.position, touch2.position);
				float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition,
				                                      touch2.position - touch2.deltaPosition);
				pinchDistanceDelta = pinchDistance - prevDistance;
				
				// Si es mayor al margen mínimo establecido, se trata de un Pinch!!
				if (Mathf.Abs(pinchDistanceDelta) > MIN_PINCH_DISTANCE) {
					pinchDistanceDelta *= PINCH_RATIO;
				} else {
					pinchDistance = pinchDistanceDelta = 0;
				}
				
				// Tambien comprobamos la variación de los ángulos entre ellos...
				turnAngle = Angle(touch1.position, touch2.position);
				float prevTurn = Angle(touch1.position - touch1.deltaPosition,
				                       touch2.position - touch2.deltaPosition);
				turnAngleDelta = Mathf.DeltaAngle(prevTurn, turnAngle);
				
				// Si es mayor al margen mínimo de rotación, es un Rotate!!
				if (Mathf.Abs(turnAngleDelta) > MIN_TURN_ANGLE) {
					turnAngleDelta *= PINCH_TURN_RATIO;
				} else {
					turnAngle = turnAngleDelta = 0;
				}
			}
		}
	}
	
	//Con esta función comprobamos el ángulo entre los dos toques, con respecto a un punto de referencia (1,0)
	static private float Angle (Vector2 pos1, Vector2 pos2) {
		Vector2 from = pos2 - pos1;
		Vector2 to = new Vector2(1, 0);
		
		float result = Vector2.Angle( from, to );
		Vector3 cross = Vector3.Cross( from, to );
		
		if (cross.z > 0) {
			result = 360f - result;
		}
		
		return result;
	}
}
