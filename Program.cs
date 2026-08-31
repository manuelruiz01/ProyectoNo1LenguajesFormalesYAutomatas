public class Program{
	private static AFD afdActual = null;
	private static bool esValido = false; 

	public static void Main(String[] args) {
		Console.WriteLine("Simulador de Automatas Finitos");
		bool salir = false;
		string opcion;  

		

		while (salir == false) {
			Console.WriteLine("Seleccione una opcion: ");
			Console.WriteLine("1. Cargar quintupla desde archivo .txt");
			Console.WriteLine("2. Cargar quintupla manualmente ");
			Console.WriteLine("3. Mostrar definicion formal y tabla de transicion");
			Console.WriteLine("4. Evaluar cadena individual");
			Console.WriteLine("5. Evaluar lote de cadenas desde archivo .txt");
			Console.WriteLine("6. Reiniciar/ Cargar nuevo automata");
			Console.WriteLine("7. Salir");
			opcion = Console.ReadLine();

			switch (opcion) {
				case "1":
					Console.WriteLine("Ingrese el nombre del archivo");
					string nombre = Console.ReadLine();
					List<string> errores = new List<string>();
					AFD afd = CargaAFD.cargarDesdeArchivo(nombre, errores);

					if (errores.Count > 0) {
						Console.WriteLine("Se encontraron errores de sintaxis en el archivo: ");
						foreach (string error in errores) {
							Console.WriteLine(error); 
						
						}
						continue;
					
					}

					if (afd == null) {
						Console.WriteLine("No se pudo cargar el automata ");
						continue;
					
					}
					afdActual = afd;
					validarAfd();

					break;

				case "2":
					afdActual = CargaAFD.cargarDesdeConsola();
					validarAfd();
					break;

				case "3": 
					if(afdActual == null && esValido == false){
						Console.WriteLine("Primero debes de cargar el afd a traves de la opcion 1 o 2");
						continue; 
					}
					afdActual.mostrarDefinicionFormal();
					afdActual.mostrarTabladeTransicion();

					break;


				case "4":
					if (afdActual == null && esValido == false){
						Console.WriteLine("Primero debes de cargar el afd correctamente a traves de la opcion 1 o 2");
						continue;
					}
					Console.WriteLine("Ingresa la cadena a evaluar");
					string cadena = Console.ReadLine();
					EvaluacionCadena resultadoCadena = ProcesamientoConAFD.evaluarCadena(afdActual,cadena);
					resultadoCadena.mostrarDetalles();
					break; 

				case "5":
					
					if (afdActual == null && esValido == false){ 

						Console.WriteLine("Primero debes de cargar el afd correctamente a traves de la opcion 1 o 2");
						continue;
					}
					Console.WriteLine("Ingresa el nombre del archivo: ");
					string nombreArchivoCadenas = Console.ReadLine();

					string errorLeerArchivo = "";

					List<string> cadenas = ProcesamientoConAFD.LeerCadenasArchivo(nombreArchivoCadenas, errorLeerArchivo);
					 
					if(errorLeerArchivo.Length > 0){
						Console.WriteLine(errorLeerArchivo);
						continue;
					 }

					List<EvaluacionCadena> resultados = ProcesamientoConAFD.evaluarConjuntoDeCadenas(afdActual, cadenas);
					Console.WriteLine("Cadenas evaluadas: " + resultados.Count); 

					foreach(EvaluacionCadena resultado in resultados){
						resultado.mostrarDetalles(); 

						
					}
					
					break;

				case "6":
					if (afdActual == null)
					{
						Console.WriteLine("Primero debes de cargar el afd a traves de la opcion 1 o 2");
						continue;
					}
					afdActual = null;
					esValido = false;
					Console.WriteLine("Automata reiniciado ");
					break;


				case "7":
					salir = true;
					Console.WriteLine("Saliendo...");
					break;

				default:
					break; 
			
			}



		}


	}

	public static void validarAfd(){
		List<string> errores = new List<string>();
		esValido = ValidacionAFD.validarTupla(afdActual, errores);  
		if(esValido){
			Console.WriteLine("El automata ingresado es un afd valido"); 

		}else{
			Console.WriteLine("Ël automata ingresado no es valido. Se encontraron los siguientes errores: "); 
			foreach(string error in errores){
				Console.WriteLine(error);

			}


		}

		
		

	}

}

