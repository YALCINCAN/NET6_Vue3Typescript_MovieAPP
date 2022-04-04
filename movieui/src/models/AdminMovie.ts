import { MovieImage } from './MovieImage';
export interface AdminMovie{
    id:number,
    name:string,
    summary:string,
    imdb:number,
    releaseDateString:string,
    categoryIds:number[],
    mainImage:string,
    actorIds:number[]
    movieImages:MovieImage[]
}