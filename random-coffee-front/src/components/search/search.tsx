import {ChangeEvent, FC, useEffect, useState} from "react";
import {Input} from '@skbkontur/react-ui';
import {Filter} from "../filter/filter";
import style from './search.module.scss';
import {TGroup} from "../../types/group";
import {Search} from '@skbkontur/react-icons';

type Props = {
    groups: TGroup[];
    onSetGroups: (value: TGroup[]) => void;
}

export const SearchGroup: FC<Props> =({groups, onSetGroups}) =>{

    const [copyData] = useState(groups);
    const [first, setFirst] = useState(0);
    const [second, setSecond] = useState(0);

    const sortDate = (paramToSort: string) => {
        let sortData: TGroup[];
        let copyData = groups.concat();

        if (paramToSort === 'users.length') {
            if (first === 0)
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
            if (second === 0)
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
        onSetGroups(sortData);
    }

    useEffect( () => {
        sortDate ('users.length');
    }, [first])
    useEffect( () => {
        sortDate ('id');
    }, [second])




    function SearchGroup(event: ChangeEvent<HTMLInputElement>){
        const target = event.target;
        const input = target.value;
        if (input === "") {
            onSetGroups(copyData);
            return;
        }
        let resultData: TGroup[] = [];
        for (let i = 0; i< copyData.length; i++){
            if (copyData[i].name.includes(input))
                resultData.push(copyData[i])
        }
        onSetGroups(resultData);
    }


    return (
        <div className={style.wrapper}>
            <div className={style.input}>
                <Input width={'100%'} placeholder={'Введите название группы'} leftIcon={<Search/>} onChange={(event) => {
                    SearchGroup(event)
                }}/>
            </div>
            <div className={style.filters}>
                <Filter orderBy={"Количество участников"} usageState={first} setUsageState={setFirst} setOtherFilterState={setSecond}/>
                <Filter orderBy={"id"} usageState={second} setUsageState={setSecond} setOtherFilterState={setFirst}/>
            </div>
        </div>
    )
}
