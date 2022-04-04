import { Category } from './Category';

export interface Movie {
  id: number;
  name: string;
  slug:string,
  mainImage: string;
  imdb: number;
  releaseDate: string;
  categories: Category[];
}
