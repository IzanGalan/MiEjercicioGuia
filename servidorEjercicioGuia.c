#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>

int main(int argc, char *argv[]) {
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
	
	// Inicializaciones
	// Abrimos el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
		printf("Error creando socket\n");
		exit(1);
	}
	
	// Configuramos la dirección del servidor
	memset(&serv_adr, 0, sizeof(serv_adr));
	serv_adr.sin_family = AF_INET;
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_adr.sin_port = htons(9080);
	
	if (bind(sock_listen, (struct sockaddr *)&serv_adr, sizeof(serv_adr)) < 0) {
		printf("Error en el bind\n");
		exit(1);
	}
	
	if (listen(sock_listen, 4) < 0) {
		printf("Error en el listen\n");
		exit(1);
	}
	
	printf("Escuchando\n");
	
	for (;;) {
		sock_conn = accept(sock_listen, NULL, NULL);
		if (sock_conn < 0) {
			printf("Error en el accept\n");
			continue;
		}
		printf("He recibido conexión\n");
		
		int terminar = 0;
		while (terminar == 0) {
			ret = read(sock_conn, peticion, sizeof(peticion) - 1);
			if (ret <= 0) {
				printf("Error leyendo petición o conexión cerrada\n");
				break;
			}
			
			peticion[ret] = '\0';
			printf("Recibida una petición: %s\n", peticion);
			
			char *p = strtok(peticion, "/");
			int codigo = atoi(p);
			
			char nombre[20] = "";
			if (codigo != 0) {
				p = strtok(NULL, "/");
				if (p != NULL) {
					strcpy(nombre, p);
				}
				printf("Codigo: %d, Nombre: %s\n", codigo, nombre);
			}
			
			if (codigo == 0) {
				terminar = 1;
			} else if (codigo == 1) {
				sprintf(respuesta, "%d", (int)strlen(nombre));
			} else if (codigo == 2) {
				if (nombre[0] == 'M' || nombre[0] == 'S') {
					strcpy(respuesta, "SI");
				} else {
					strcpy(respuesta, "NO");
				}
			} else if (codigo == 3) {
				p = strtok(NULL, "/");
				float altura = p ? atof(p) : 0;
				if (altura > 1.70) {
					sprintf(respuesta, "%s: eres alto", nombre);
				} else {
					sprintf(respuesta, "%s: eres bajo", nombre);
				}
			} else {
				strcpy(respuesta, "Codigo desconocido");
			}
			
			if (codigo != 0) {
				printf("Respuesta: %s\n", respuesta);
				write(sock_conn, respuesta, strlen(respuesta) + 1);
			}
		}
		close(sock_conn);
	}
}
