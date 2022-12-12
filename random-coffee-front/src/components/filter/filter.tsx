import {FC} from "react";
import style from './filter.module.scss';


type Props = {
    orderBy: string;
    usageState: number;
    setUsageState: (value: number) => void;
    setOtherFilterState: (value: number) => void;
}

type FType = (stateNow: number, setState: (value: number) => void, setOtherState: (value: number) => void) => void

export const Filter: FC<Props> =({orderBy= 'Что-то', usageState, setUsageState, setOtherFilterState}) =>{

    const MakeFilter = (...args: Parameters<FType>): ReturnType<FType> => {
        const [stateNow, setState, setOtherState] = args;
        let newState = stateNow + 1 === 3? 0 : stateNow + 1;
        setState(newState);
        setOtherState(0);
    }

    return (
        <div className={style.wrapper} onClick={()=>{MakeFilter(usageState, setUsageState, setOtherFilterState)}}>
            <span>{orderBy}{usageState === 1 && '▲'}{usageState === 2 && '▼'}</span>
        </div>
    )
}