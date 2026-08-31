using System;
using System.Collections.Generic;
using System.Text;

public class Transicion {
	public string estadoOrigen;
	public string simbolo;
	public string estadoDestino;
	public Transicion(string estadoOrigen, string simbolo, string estadoDestino) {
		this.estadoOrigen = estadoOrigen;
		this.simbolo = simbolo;
		this.estadoDestino = estadoDestino;
	}


}
