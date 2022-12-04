import {FC, useEffect, useState} from "react";
import {Input} from '@skbkontur/react-ui';
import {Filter} from "../filter/filter";
import style from './search.module.scss';
import {TGroup} from "../../types/group";

type Props = {
    groups: TGroup[];
    setGroups: (value: TGroup[]) => void;
}

export const Search: FC<Props> =({groups, setGroups}) =>{
    
    const [first, setFirst] = useState(0);
    const [second, setSecond] = useState(0);
    useEffect( () => {
        sortDate ('users.length');
    }, [first])
    useEffect( () => {
        sortDate ('id');
    }, [second])


    function sortDate(paramToSort: string) : void{
        const copyData = groups.concat();
        let sortData: TGroup[];


        if (paramToSort === 'users.length') {
            if (first == 0)
                return;
            sortData = first === 1  ? copyData.sort(
                (a, b) => { // @ts-ignore
                    return a.users.length > b.users.length ? 1 : -1
                }
            ) : copyData.sort(
                (a, b) => { // @ts-ignore
                    return a.users.length > b.users.length ? -1 : 1
                })
        }
        else {
            if (second == 0)
                return;
            sortData = second === 1 ? copyData.sort(
                (a, b) => { // @ts-ignore
                    return a[paramToSort] > b[paramToSort] ? 1 : -1
                }
            ) : copyData.sort(
                (a, b) => { // @ts-ignore
                    return a[paramToSort] > b[paramToSort] ? -1 : 1
                }
            )
        }
        setGroups(sortData);
    }

    return (
        <div className={style.wrapper}>
            <div className={style.input}>
                <Input width={'100%'} placeholder={'Введите название группы'}/>
            </div>
            <div className={style.filters}>
                <Filter name={"Количество участников"}  state={first} setState={setFirst} setOtherState={setSecond}/>
                <Filter name={"id"} state={second} setState={setSecond} setOtherState={setFirst}/>
            </div>
        </div>
    )
}