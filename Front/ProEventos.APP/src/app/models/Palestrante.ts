import { User } from 'src/app/models/Users/User';
import { Evento } from "./Evento";
import { RedeSocial } from "./RedeSocial"

export interface Palestrante {
  id: number;
  user: User;
  miniCurriculo: string;
  redeSociais: RedeSocial[];
  PalestrantesEventos: Evento[];
}
