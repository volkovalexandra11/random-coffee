import { GroupPage } from './group-page';
import { mockGroupList } from '../mock/mock-group-list';
import styles from '/frontend/random-coffee/styles/Home.module.scss'


export default function Home() {
  return (<GroupPage groupList={mockGroupList}/>);
}
