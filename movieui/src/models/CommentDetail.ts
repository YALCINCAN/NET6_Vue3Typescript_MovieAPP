import { User } from './User';
export interface CommentDetail{
    id:number,
    description:string,
    commentDate:Date,
    userId:string,
    user:User
}