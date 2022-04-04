export interface AddMovie{
    name:string,
    summary:string,
    imdb:number,
    releaseDateString:string,
    categoryIds:number[],
    actorIds:number[]
}