using System;
using System.Collections.Generic;
using System.Text;

public class ProcesamientoConAFD{
	//Lector de las cadenas que estan en el archivo .txt. La cadena de cada linea 

	public static List<string> LeerCadenasArchivo(string nombre, string error){

		List<string> cadenas = new List<string>();
		string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombre); 

		if(!File.Exists(ruta)){
			error = "No se encontro el archivo";
			return cadenas;
		}

		string[] lineas = File.ReadAllLines(ruta);
		//Para asegura que no se agregen cadenas vacias a las cadenas a evaluar y asegurar que las cadenas a evaluar no tengan ningun espacio en blanco al inicio o al final 
		//y que esto cause errores falsos
		foreach(string linea in lineas){
			string cadena = linea.Trim();
			if(lineas.Length > 0){
				cadenas.Add(linea);
			}
			

		}

		return cadenas;

	} 

	//Se evalua una cadena con base a las transiciones y alfabeto almacenados en el afd

	public static EvaluacionCadena evaluarCadena(AFD afd, string cadena){
		
		EvaluacionCadena resultado = new EvaluacionCadena(cadena);
		string estadoActual = afd.estadoInicial;
		string siguienteEstado;

		//se verifica que todos los simbolos dentro de la cadena esten el afd
		for(int i = 0; i<cadena.Length; i++){
			string simbolo = cadena[i].ToString(); 
			if(afd.alfabeto.Contains(simbolo) == false){
				resultado.error = "El simbolo " + simbolo + " no existe en el alfabeto";
				return resultado; 
			}

			//funcion para colocar en el formato (estadoActual|simbolo, estadoSiguiente), para verficar si existe cada transicion con los simbolos de la cadena
			string clave = AFD.claveDelta(estadoActual, simbolo); 

			//Se verifica que la transicion exista de manera que exista un estado destino para el estado actual con el simbolo actual leido en la cadena
			if(afd.delta.TryGetValue(clave, out siguienteEstado) == false){
				resultado.error = "No existe transicion para " + estadoActual + "-"+simbolo + "->";
				return resultado;
			}

			resultado.rastro.Add(new Transicion(estadoActual, simbolo, siguienteEstado));
			estadoActual = siguienteEstado;
		}

		//se verifica que realmente el ultimo estado alcanzado sea el estado final para dar el verdicto final si la cadena es aceptada o no por el afd
		resultado.estadoFinal = estadoActual;
		resultado.aceptada = afd.estadosFinales.Contains(estadoActual);
		return resultado;

	}

	//Usada para evaluar el conjunto de cadenas que vienen el archivo txt. Se une la evaluacion de cada cadena ubicada en cada linea 
	public static List<EvaluacionCadena> evaluarConjuntoDeCadenas(AFD afd, List<string>cadenas){
		List<EvaluacionCadena> resultados = new List<EvaluacionCadena>(); 
		
		foreach(string cadena in cadenas){
			EvaluacionCadena resultado = evaluarCadena(afd, cadena);
			resultados.Add(resultado);
		}
		return resultados;
	} 

	


}