import { TGroupDto } from '../types/group';
import {
    IS_PRIVATE_CONST,
    ONCE_A_MONTH_CONST,
    ONCE_A_WEAK_CONST,
    SET_MANUALLY_CONST
} from '../constants/text-constants';

type TempObj = {
    groupName: string,
    description: string,
    groupType: string,
    repeatMeetings: string,
    meetingDate: string;
};
export const getGroupDto = (data: TempObj): TGroupDto => {
    const isPrivate = getIsPrivate(data.groupType);
    const frequency = getFrequency(data.repeatMeetings);
    const nextRoundDate = getISODate(data.meetingDate);
    return { name: data.groupName, isPrivate, nextRoundDate, intervalDays: frequency };
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
