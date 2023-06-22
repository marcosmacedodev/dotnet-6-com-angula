import { Lote } from "./Lote"
import { Palestrante } from "./Palestrante";
import { RedeSocial } from "./RedeSocial"

export interface Evento {

   id: number;
   local: string;
   dataEvento?: Date;
   tema: string;
   qtdPessoas: number;
   lotes: Lote[];
   imagemUrl: string;
   telefone: string;
   redesSociais: RedeSocial[];
   PalestrantesEventos: Palestrante[];
}
