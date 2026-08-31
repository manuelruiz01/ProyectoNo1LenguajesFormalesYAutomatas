using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

public class CargaAFD{
	public static AFD cargarDesdeArchivo(string nombreArchivo, List<string> errores) {
		string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombreArchivo);

		if (!File.Exists(ruta)) {
			errores.Add("No se encontro el archivo " + nombreArchivo);
			return null;
		
		}

		string[] lineas = File.ReadAllLines(ruta);
		AFD afd = new AFD();
		bool bloqueT = false;

		foreach (string lineaOriginal in lineas) {
			string linea = lineaOriginal.Trim();

			if (linea.Length == 0) {
				continue; //Si hay algun enter (linea vacia) se ignora
			
			}

			//Se extrae el contenido de cada una de las tuplas y dicho contenido se asigna a su atributo correspondiente en la clase afd 
			if (bloqueT == false)
			{
				if (linea.StartsWith("Q"))
				{
					List<string> elementos = extraerConjunto(linea, errores);
					afd.estados = listaAHashSet(elementos);

				}
				else if (linea.StartsWith("A"))
				{
					List<string> elementos = extraerConjunto(linea, errores);
					afd.alfabeto = listaAHashSet(elementos);

				}
				else if (linea.StartsWith("S"))
				{
					afd.estadoInicial = extraerValorSimple(linea, errores);

				}
				else if (linea.StartsWith("F"))
				{
					List<string> elementos = extraerConjunto(linea, errores);
					afd.estadosFinales = listaAHashSet(elementos);

				}
				else if (linea.StartsWith("T"))
				{
					if (linea.Contains("{"))
					{
						bloqueT = true;
					}
					else
					{
						errores.Add("Formato no valido para T en: " + lineaOriginal);
					}
				}
				else
				{
					errores.Add("Linea no reconocida en: " + lineaOriginal);
				}


			} 
			//dentro de T = {}
			else {
				if (linea.StartsWith("}"))
				{
					bloqueT = false;
				}
				else {
					Transicion t = parserTransicion(linea, errores);
					if (t != null) {
						afd.transiciones.Add(t);
					
					}
				
				}
			
			}
		}
		return afd;

	}

	private static HashSet<string> listaAHashSet(List<string> lista) {
		HashSet<string> conjunto = new HashSet<string>();
		if (lista == null) {
			return conjunto; 
		
		}
		foreach (string elemento in lista) {
			conjunto.Add(elemento); 

		}
		return conjunto;

	
	}

	//Extrae los elementos de cada conjunto de la tupla y si detecta errores los señala 
	//El formato valido es: Nombre del conjunto = {elemento1,elemento2,...}
	private static List<string> extraerConjunto(string linea, List<string> errores) {
		List<string> resultado = new List<string>();
		int posIgual = linea.IndexOf('=');
		int posInicio = linea.IndexOf('{');
		int posFinal = linea.IndexOf('}');

		if (posIgual < 0 || posInicio < 0 || posFinal < 0) {
			errores.Add("Formato de conjunto no valido: " + linea);
			return resultado; 
		
		}

		string contenido = linea.Substring(posInicio + 1, posFinal - posInicio - 1);
		string[] partes = contenido.Split(',');

		for (int i = 0; i < partes.Length; i++) {
			string elemento = partes[i].Trim();
			if (elemento.Length > 0) {
				resultado.Add(elemento);
			
			}
		
		}

		if (resultado.Count == 0) {
			errores.Add("Conjunto vacio en la linea: " + linea); 
		
		}
		return resultado; 

	
	}

	//Sirve para verificar si el estado inicial esta correctamente indicado
	private static string extraerValorSimple(string linea, List<string> errores) {
		int posIgual = linea.IndexOf('=');

		if (posIgual < 0 || posIgual == linea.Length - 1) {
			errores.Add("Formato invalido en: " + linea);
			return "";
		}

		string valor = linea.Substring(posIgual + 1).Trim();
		if (valor.Length == 0) {
			errores.Add("Valor vacio en : " + linea);
		
		}
		return valor; 
	
	}

	//Sirve para verificar que las transiciones esten correctamente indicadas dentro del txt
	private static Transicion parserTransicion(string linea, List<string> errores) {
		int posParInicial = linea.IndexOf('(');
		int posParFinal = linea.IndexOf(')');
		int posFlecha = linea.IndexOf("->");

		if (posParInicial < 0 || posParFinal < 0 || posParFinal < posParInicial) {
			errores.Add("Formato no valido para transicion en: " + linea);
			return null; 
		
		}

		string par = linea.Substring(posParInicial + 1, posParFinal - posParInicial - 1);
		string[] partes = par.Split(',');

		if (partes.Length != 2) {
			errores.Add("Par (estado, simbolo) invalido en: " + linea);
			return null; 
		
		}

		string origen = partes[0].Trim();
		string simbolo = partes[1].Trim();
		string destino = linea.Substring(posFlecha + 2).Trim();
		destino = destino.TrimEnd(',', ' ', '}');

		if (origen.Length == 0 || simbolo.Length == 0 || destino.Length == 0) {
			errores.Add("Transicion incompleta en : " + linea);
			return null;
		
		}

		return new Transicion(origen, simbolo, destino);


	}

	//Carga manual de AFD (carga desde consola) 
	public static AFD cargarDesdeConsola(){
		AFD afd = new AFD();

		Console.WriteLine("Ingrese los estados del conjunto Q, separados por coma ");
		string estados = Console.ReadLine();
		afd.estados = stringAconjunto(estados);
		
		Console.WriteLine("Ingrese los simbolos del alfabeto, separados por coma ");
		string alfabeto = Console.ReadLine();
		afd.alfabeto = stringAconjunto(alfabeto);

		Console.WriteLine("Ingrese el estado inicial ");
		afd.estadoInicial = Console.ReadLine().Trim();

		Console.WriteLine("Ingrese los estados finales F, separados por coma ");
		string estadosFinales = Console.ReadLine();
		afd.estadosFinales = stringAconjunto(estadosFinales);

		Console.WriteLine("Ingreso de transiciones ");
		Console.WriteLine("Ingresalas de la forma: estadoInicial,simbolo,estadoFinal. Con comas");
		Console.WriteLine("Para terminar de ingresar transiciones escribe la letra 's'");

		bool terminado = false; 
		//Se ingresan las transiciones y se va validando que esten en el formato requerido para que los elementos de estas puedan ser almacenados en un objeto que representa a un AFD
		while(terminado == false){
			Console.WriteLine("Ingresa transicion ");
			string entrada = Console.ReadLine().Trim(); 

			if(entrada.ToLower() == "s"){
				terminado = true; 

			}else{
				string[] elementos = entrada.Split(','); 
				if(elementos.Length != 3){
					Console.WriteLine("Formato de ingreso de transiciones no valido");
				}else{

					string origen = elementos[0].Trim();
					string simbolo = elementos[1].Trim();
					string destino = elementos[2].Trim();

					afd.transiciones.Add(new Transicion(origen, simbolo, destino));


				}

			}

		}
		return afd; 
	}

	public static HashSet<string> stringAconjunto(string cadena){
		HashSet<string> conjunto = new HashSet<string>();
		string[] partes = cadena.Split(','); 

		for(int i = 0; i<partes.Length; i++){
			string elemento = partes[i].Trim(); 
			if(elemento.Length >0){
				conjunto.Add(elemento);

			}

		}
		return conjunto; 
		

	}



}