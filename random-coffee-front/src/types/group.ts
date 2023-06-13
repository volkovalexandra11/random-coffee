import { TUser } from './user';

export type TGroupShort = {
    groupId: string;
    name: string;
    participantsCount: number;
    tag: string[] | null;
    groupPictureUrl?: string;
    nextRoundDate: string;
};

export type TGroup = {
    groupId: string;
    name: string;
    admin: TUser;
    participantsCount: number;
    groupPictureUrl?: string;
    participants: TUser[];
    description: string;
    isPrivate: boolean;
};

export type TGroupDto = {
    name: string;
    isPrivate: boolean;
    nextRoundDate: string;
    intervalDays: number;
    groupPictureUrl?: string;
}
