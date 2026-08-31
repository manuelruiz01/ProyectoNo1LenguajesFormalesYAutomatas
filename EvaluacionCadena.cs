using System;
using System.Collections.Generic;
using System.Text;

//Contiene cada uno de los pasos en la evaluacion de cada cadena, los errores que se encuentren en la cadena si esta no es aceptada
//y el veredicto final que indica si al evaluar una cadena se llego al estado final 
public class EvaluacionCadena{
	public string cadena;
	public List<Transicion> rastro;
	public string estadoFinal;
	public bool aceptada;
	public string error;  

	public EvaluacionCadena(string cadena){
		this.cadena = cadena;
		this.rastro = new List<Transicion>();
		this.estadoFinal = "";
		this.error = null; 

	} 

	public void mostrarDetalles(){
		Console.WriteLine("Cadena evaluada: " + cadena); 

		if(cadena.Length == 0){
			Console.WriteLine("Cadena vacia ");  

		}

		
		Console.WriteLine("Camino tomado dentro del automata para evaluar la cadena: ");
		foreach (Transicion paso in rastro){
			Console.WriteLine("Estado Actual: " + paso.estadoOrigen + " |Simbolo: " + paso.simbolo + " |Siguiente estado: " + paso.estadoDestino);
		} 

		if(error != null){
			Console.WriteLine(error);
		} 

		if(aceptada == true){
			Console.WriteLine("Veredicto final: Cadena aceptada "); 

		}else{
			Console.WriteLine("Veredicto final: Cadena rechazada ");

		}

	}


}
