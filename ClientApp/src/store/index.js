import {createStore} from "vuex";
import {requestModule} from "@/store/requestModule";

export default createStore({
  modules: {
    requestModule: requestModule,
  }
});