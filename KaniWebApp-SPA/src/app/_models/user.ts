import { Image } from './image';

export interface User {
    id: number;
    username: string;
    nickname: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    imageUrl: string;
    city: string;
    country: string;
    aboutMe?: string;
    images?: Image[];
}
