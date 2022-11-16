import style from './nav-bar.module.scss'
import { FC } from 'react';
import {TUser} from "../../types/user";


export const NavBar: FC<TUser> = (props) => {
    return (
        <>
            <div className={style.navBar}>
                <div className={style.logo}>
                    <a href={''}><h2>Cлучайный кофе</h2></a>
                </div>
                <div className={style.userPanel}>
                    <button className={style.buttons}>
                        <div className={style.test}>
                            {/*<img src={'img/bell.png'} alt={''}/>*/}
                        </div>
                    </button>
                    <a className={style.buttons} href={''}>
                        <span className={style.text}>{props.firstName+" "+ props.lastName}</span>
                    </a>
                    <span className={style.line}/>
                    <button className={style.exit} onClick={ () => alert('Logout!')}>
                        <span className={style.text}>Выйти</span>
                    </button>
                </div>
            </div>
            <div style={{paddingTop: 50}}/>
        </>)
}