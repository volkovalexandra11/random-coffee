import { TGroupDto } from '../types/group';
import {
    IS_PRIVATE_CONST,
    ONCE_A_MONTH_CONST,
    ONCE_A_WEAK_CONST,
    SET_MANUALLY_CONST
} from '../constants/text-constants';
import { getRandomImg } from './randomHelper';

type TempObj = {
    groupName: string,
    groupType: string,
    repeatMeetings: string,
    meetingDate: string,
    description?: string,
};

export const getGroupDto = (data: TempObj): TGroupDto => {
    const isPrivate = getIsPrivate(data.groupType);
    //const frequency = getFrequency(data.repeatMeetings);
    const frequency = 7;
    const nextRoundDate = getISODate(data.meetingDate);
    const description = data.description;
    const pictureUrl = `/img/${getRandomImg()}`;
    const tags = '';
    return { name: data.groupName, isPrivate, nextRoundDate, intervalDays: frequency, groupPictureUrl: pictureUrl, groupDescription: description, tag: tags };
};

const getIsPrivate = (literal: string): boolean => {
    return literal === IS_PRIVATE_CONST;
};

const getFrequency = (literal: string): number => {
    switch (literal) {
        case ONCE_A_WEAK_CONST:
            return 7;
        case ONCE_A_MONTH_CONST:
            return 30;
        case SET_MANUALLY_CONST:
            return 0;
        default:
            return -1;
    }
};

const getISODate = (literal: string) : string => {
    const parts = literal.split('.');
    console.log(parts);
    const newDateString = `${parts[1]} ${parts[0]} ${parts[2]}`;
    console.log(newDateString, new Date(newDateString));
    return new Date(newDateString).toISOString()
}
