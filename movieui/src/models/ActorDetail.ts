import { Movie } from './Movie';

export interface ActorDetail {
  id: number;
  fullName: string;
  about: string;
  image: string;
  slug: string;
  movies: Movie[];
}
