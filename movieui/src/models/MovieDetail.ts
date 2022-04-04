import { MovieImage } from './MovieImage';
import { Actor } from './Actor';
import { Category } from './Category';
import { CommentDetail } from './CommentDetail';

export interface MovieDetail {
    id:number,
    name:string,
    mainImage: string;
    imdb:number,
    summary:string,
    releaseDate:string,
    categories:Category[]
    actors:Actor[]
    images:MovieImage[],
    comments:CommentDetail[]
}