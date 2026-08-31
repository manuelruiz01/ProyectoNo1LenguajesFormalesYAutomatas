using System;
using System.Collections.Generic;
using System.Text;

public class AFD{
	public HashSet<string> estados;
	public HashSet<string> alfabeto;
	public List<Transicion> transiciones;
	public Dictionary<string, string> delta; //Funcion de transicion que guarda el estado inicial y el simbolo y el estado al que pasa  
	//Lo guardara de la forma: (estadoInicial|simbolo:estadoDestino)
	public string estadoInicial;
	public HashSet<string> estadosFinales;

	public AFD() {
		estados = new HashSet<string>();
		alfabeto = new HashSet<string>();
		transiciones = new List<Transicion>();
		delta = new Dictionary<string, string>();
		estadosFinales = new HashSet<string>();
		estadoInicial = "";
	}

	//Se utilizara para unir un estado y un simblo para luego meterlo al diccionario de transiciones que representa la funcion de transicion
	public static string claveDelta(string estado, string simbolo) {
		return estado + "|" + simbolo;
	}

	private static string conjuntoAString(HashSet<string> conjunto) {
		string texto = "";
		bool esPrimero = true;

		foreach (string elemento in conjunto) {
			if (esPrimero)
			{
				texto = elemento;
				esPrimero = false;
			}
			else {
				texto = texto + "," + elemento;
			
			}
		
		}
		return texto; 
	}

	public void mostrarDefinicionFormal() {
		Console.WriteLine("Definicion formal del automata: ");
		Console.WriteLine("Q = {"+ conjuntoAString(estados) + "}");
		Console.WriteLine("Alfabeto = {" + conjuntoAString(alfabeto) + "}");
		Console.WriteLine("S = " + estadoInicial);
		Console.WriteLine("F = {" + conjuntoAString(estadosFinales) + "}");

	}

	public void mostrarTabladeTransicion(){
		Console.WriteLine("Tabla de transicion");

		//Mostrar el encabezado 
		Console.Write("Q|".PadRight(10)); 
		foreach(string simbolo in alfabeto){
			Console.Write(simbolo.PadRight(10));
		}
		Console.WriteLine();

		//Mostrar estados 
		foreach(string estado in estados){
			Console.Write(estado.PadRight(10));

			foreach (string simbolo in alfabeto){
				string inicialYsimbolo = claveDelta(estado, simbolo);
				string destino;

				if(delta.TryGetValue(inicialYsimbolo, out destino)){
					Console.Write(destino.PadRight(10));

				} else{
					Console.Write("null".PadRight(10));
				}
				
			}
			Console.WriteLine();
			
		}
		Console.WriteLine(); 
		

	}



}