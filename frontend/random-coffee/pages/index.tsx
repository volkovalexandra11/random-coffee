import { GroupPage } from './group-page/group-page';
import { mockGroupList } from '../mock/mock-group-list';


export default function Home() {
  return (<GroupPage groupList={mockGroupList}/>);
}
