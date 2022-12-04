import {FC} from "react";
import style from './filter.module.scss';


type Props = {
    name: string;
    state: number;
    setState: (value: number) => void;
    setOtherState: (value: number) => void;
}


export const Filter: FC<Props> =({name= 'Что-то', state, setState, setOtherState}) =>{

    function useFilter(stateNow: number, setState: (value: number) => void, setOtherState: (value: number) => void) {
        let newState = stateNow + 1 == 3? 0 : stateNow + 1;
        setState(newState);
        setOtherState(0);
    }

    return (
        <div className={style.wrapper} onClick={()=>{useFilter(state, setState, setOtherState)}}>
            <span>{name}{state === 1 && '▲'}{state === 2 && '▼'}</span>
        </div>
    )
}