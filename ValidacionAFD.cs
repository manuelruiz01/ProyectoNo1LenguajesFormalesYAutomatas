using System;
using System.Collections.Generic;
using System.Text;

public class ValidacionAFD{
	public static bool validarTupla(AFD afd, List<string> errores) {
		bool primeraValidacion = validarEstados(afd, errores);
		bool segundaValidacion = validarTransiciones(afd, errores); 

		//Si el automata cumple con las dos validaciones anteriores, se valida si es determinista
		if(primeraValidacion == true && segundaValidacion == true){
			bool tercerValidacion = validarDeterminismo(afd, errores);
			return tercerValidacion;
		}

		return false; 
		
	
	}

	//Valida que los estados finales y el estado inicial realmente pretenezcan a Q
	private static bool validarEstados(AFD afd, List<string> errores)
	{
		bool valido = true;

		if (afd.estadoInicial == "" || afd.estados.Contains(afd.estadoInicial) == false)
		{

			errores.Add("El estado inicial " + afd.estadoInicial + " no pertenece a Q");
			valido = false;


		}

		foreach (string estadoFinal in afd.estadosFinales)
		{
			if (afd.estados.Contains(estadoFinal) == false)
			{
				errores.Add("El estado final " + estadoFinal + "no pertence a Q");
				valido = false;

			}

		}
		return valido;
	}

	//Se valida la consistencia de transiciones evaluando los conjuntos  de estados y alfabeto y se compara con lo que contiene cada transicion
	private static bool validarTransiciones(AFD afd, List<string> errores) {
		bool valido = true;
		foreach (Transicion t in afd.transiciones) {
			if (afd.estados.Contains(t.estadoOrigen) == false) {
				errores.Add("Transicion invalida, ya que " + t.simbolo + " no pertenece a Q");
				valido = false; 
			}
			if (afd.alfabeto.Contains(t.simbolo) == false) {
				errores.Add("Transicion invalida, ya que " + t.simbolo + " no pertenece al alfabeto");
				valido = false; 
			}
			if (afd.estados.Contains(t.estadoDestino) == false)
			{
				errores.Add("Transicion invalida, ya que " + t.estadoDestino + " no pertenece a Q");
				valido = false;
			}

		}

		return valido;
	
	}

	//Se verifica si el automata es determinista evaluando cada transicion
	private static bool validarDeterminismo(AFD afd, List<string> errores) {
		bool valido = true; 

		//Se cuentan las veces que aparece cada par (estado, simbolo)
		Dictionary<string, int> conteo = new Dictionary<string, int>();
		Dictionary<string, string> mapa = new Dictionary<string, string>(); 

		foreach(Transicion t in afd.transiciones){
			string clave = AFD.claveDelta(t.estadoOrigen, t.simbolo); 
			if(conteo.ContainsKey(clave)){
				conteo[clave] = conteo[clave] + 1; 
			}else{
				conteo[clave] = 1;
				mapa[clave] = t.estadoDestino;

			}
		
		}

		//Verificar la existencia de transicion multples para el mismo par, de manera que si existen el automata ya no es un afd
		//De manera que si hay dos transiciones con el mismo estado inicial y el mismo simbolo, el automata ya no es un afd, porque quiere decir que hay dos caminos para un mismo simbolo 
		foreach(string clave in conteo.Keys){
			if(conteo[clave] > 1){
				errores.Add("Transicion multiple para " + clave + " invalida");

			}

		}

		//Verificar si existen transiciones vacias, de manera que si existen el automata ya no es un afd
		foreach(string estado in afd.estados){
			foreach(string simbolo in afd.alfabeto){
				string clave = AFD.claveDelta(estado, simbolo);
				if(conteo.ContainsKey(clave) == false){
					errores.Add("No existe transicion para: (" + estado + "," + simbolo + ")");
					valido = false; 
	
				}
			}

		}
		if(valido == true){
			afd.delta = mapa;

		}
		return valido; 

	} 



}
